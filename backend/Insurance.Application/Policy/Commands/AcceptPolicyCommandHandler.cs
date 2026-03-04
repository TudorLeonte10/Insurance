using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Messaging;
using Insurance.Application.Events;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.Services;
using Insurance.Domain.Policies;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Commands
{
    public class AcceptPolicyCommandHandler : IRequestHandler<AcceptPolicyCommand, Guid>
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public AcceptPolicyCommandHandler(IPolicyRepository policyRepository, IUnitOfWork unitOfWork, IIntegrationEventPublisher eventPublisher)
        {
            _policyRepository = policyRepository;
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }

        public async Task<Guid> Handle(AcceptPolicyCommand request, CancellationToken cancellationToken)
        {
            var policy = await _policyRepository.GetByIdAsync(request.PolicyId, cancellationToken);

            if (policy == null)
            {
                throw new NotFoundException($"Policy with ID {request.PolicyId} not found.");
            }

            policy.SetToDraft();
            await _policyRepository.UpdateAsync(policy, cancellationToken);

            var integrationEvent = new PolicyStatusChangedIntegrationEvent(policy.Id, policy.Status.ToString(), DateTime.UtcNow);

            await _eventPublisher.Publish(integrationEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Id;
        }
    }
}
