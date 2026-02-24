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
    public class CancelPolicyCommandHandler : IRequestHandler<CancelPolicyCommand, Guid>
    {
        private readonly IPolicyRepository _policyRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IIntegrationEventPublisher _eventPublisher;
        private readonly TimeProvider _timeProvider;
        public CancelPolicyCommandHandler(IPolicyRepository policyRepo, IUnitOfWork unitOfWork, ICurrentUserContext currentUserContext, IIntegrationEventPublisher eventPublisher, TimeProvider timeProvider)
        {
            _policyRepo = policyRepo;
            _unitOfWork = unitOfWork;
            _currentUserContext = currentUserContext;
            _eventPublisher = eventPublisher;
            _timeProvider = timeProvider;
        }
        public async Task<Guid> Handle(CancelPolicyCommand request, CancellationToken cancellationToken)
        {
            var brokerId = _currentUserContext.BrokerId;

            var policy = await _policyRepo.GetByIdAsync(request.policyId, cancellationToken);

            if (policy == null)
            {
                throw new NotFoundException($"Policy with Id {request.policyId} was not found.");
            }

            if (policy.BrokerId != brokerId)
            {
                throw new ForbiddenException("Policy does not belong to the current broker");
            }

            policy.Cancel(request.cancellationReason, _timeProvider.GetUtcNow().UtcDateTime);
            await _policyRepo.UpdateAsync(policy, cancellationToken);

            var integrationEvent = new PolicyStatusChangedIntegrationEvent(
                policy.Id,
                policy.Status.ToString(),
                _timeProvider.GetUtcNow().UtcDateTime);

            await _eventPublisher.Publish(integrationEvent, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Id;
        }
    }
}
