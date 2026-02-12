using Insurance.Application.Abstractions;
using Insurance.Application.Authentication;
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
        public CancelPolicyCommandHandler(IPolicyRepository policyRepo, IUnitOfWork unitOfWork, ICurrentUserContext currentUserContext)
        {
            _policyRepo = policyRepo;
            _unitOfWork = unitOfWork;
            _currentUserContext = currentUserContext;
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

            policy.Cancel(request.cancellationReason);
            await _policyRepo.UpdateAsync(policy, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Id;
        }
    }
}
