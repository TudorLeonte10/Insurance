using Insurance.Application.Abstractions;
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
        public ActivatePolicyCommandHandler(IPolicyRepository policyRepo, IUnitOfWork unitOfWork)
        {
            _policyRepo = policyRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(ActivatePolicyCommand request, CancellationToken cancellationToken)
        {
            var policy = await _policyRepo.GetByIdAsync(request.policyId, cancellationToken);

            if(policy == null)
            {
                throw new NotFoundException($"Policy with Id {request.policyId} was not found.");
            }

            policy.Activate(DateTime.UtcNow);
            await _policyRepo.UpdateAsync(policy, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Id;
        }
    }
}
