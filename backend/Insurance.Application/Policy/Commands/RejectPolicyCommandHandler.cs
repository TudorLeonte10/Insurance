using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Messaging;
using Insurance.Application.Events;
using Insurance.Application.Exceptions;
using Insurance.Domain.Policies;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Commands
{
    public class RejectPolicyCommandHandler : IRequestHandler<RejectPolicyCommand, Guid>
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public RejectPolicyCommandHandler(IPolicyRepository policyRepository, IUnitOfWork unitOfWork, IIntegrationEventPublisher eventPublisher)
        {
            _policyRepository = policyRepository;
            _unitOfWork = unitOfWork;
            _eventPublisher = eventPublisher;
        }
        public async Task<Guid> Handle(RejectPolicyCommand request, CancellationToken cancellationToken)
        {
            var policy = await _policyRepository.GetByIdAsync(request.PolicyId, cancellationToken);

            if (policy is null)
            {
                throw new NotFoundException($"Policy with id {request.PolicyId} not found.");
            }

            policy.Reject();
            await _policyRepository.UpdateAsync(policy, cancellationToken);

            var integrationEvent = new PolicyStatusChangedIntegrationEvent(policy.Id, policy.Status.ToString(), DateTime.UtcNow);
            
            await _eventPublisher.Publish(integrationEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return policy.Id;
        }
    }
}
