using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Messaging;
using Insurance.Application.Authentication;
using Insurance.Application.Events;
using Insurance.Application.Exceptions;
using Insurance.Domain.Policies;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Commands
{
    public class ActivatePolicyCommandHandler : IRequestHandler<ActivatePolicyCommand, Guid>
    {
        private readonly IPolicyRepository _policyRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IIntegrationEventPublisher _eventPublisher;
        public ActivatePolicyCommandHandler(IPolicyRepository policyRepo, IUnitOfWork unitOfWork, ICurrentUserContext currentUserContext, IIntegrationEventPublisher eventPublisher)
        {
            _policyRepo = policyRepo;
            _unitOfWork = unitOfWork;
            _currentUserContext = currentUserContext;
            _eventPublisher = eventPublisher;
        }
        public async Task<Guid> Handle(ActivatePolicyCommand request, CancellationToken cancellationToken)
        {
            var brokerId = _currentUserContext.BrokerId;

            var policy = await _policyRepo.GetByIdAsync(request.policyId, cancellationToken);

            if(policy == null)
            {
                throw new NotFoundException($"Policy with Id {request.policyId} was not found.");
            }

            if(policy.BrokerId != brokerId)
            {
                throw new ForbiddenException("Policy does not belong to the current broker");
            }

            policy.Activate(DateTime.UtcNow);
            await _policyRepo.UpdateAsync(policy, cancellationToken);

            var integrationEvent = new PolicyStatusChangedIntegrationEvent(
                policy.Id,
                policy.Status.ToString(),
                DateTime.UtcNow);
            
            await _eventPublisher.Publish(integrationEvent, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Id;
        }
    }
}
