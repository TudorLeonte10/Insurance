using Insurance.Application.Abstractions;
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
        public CancelPolicyCommandHandler(IPolicyRepository policyRepo, IUnitOfWork unitOfWork)
        {
            _policyRepo = policyRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CancelPolicyCommand request, CancellationToken cancellationToken)
        {
            var policy = await _policyRepo.GetByIdAsync(request.policyId, cancellationToken);

            if (policy == null)
            {
                throw new NotFoundException($"Policy with Id {request.policyId} was not found.");
            }

            policy.Cancel(request.cancellationReason);
            await _policyRepo.UpdateAsync(policy, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Id;
        }
    }
}
