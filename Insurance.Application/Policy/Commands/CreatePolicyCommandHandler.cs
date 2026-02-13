using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Application.Policy.Services;
using Insurance.Domain.Brokers;
using Insurance.Domain.Clients;
using Insurance.Domain.Exceptions;
using Insurance.Domain.Metadata;
using Insurance.Domain.Policies;
using Insurance.Domain.RiskIndicators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Insurance.Application.Policy.Commands
{
    public class CreatePolicyCommandHandler : IRequestHandler<CreatePolicyCommand, Guid>
    {

        private readonly IPolicyCreationService _policyCreationService;
        private readonly IPolicyRepository _policyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserContext _currentUserContext;

        public CreatePolicyCommandHandler(
            IPolicyCreationService policyCreationService,
            IPolicyRepository policyRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserContext currentUserContext)
        {
            _policyCreationService = policyCreationService;
            _policyRepository = policyRepository;
            _unitOfWork = unitOfWork;
            _currentUserContext = currentUserContext;
        }

        public async Task<Guid> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            var brokerId = _currentUserContext.BrokerId!.Value;

            var policy = await _policyCreationService.CreatePolicyAsync(request.PolicyDto, brokerId, cancellationToken);

            await _policyRepository.AddAsync(policy, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Id;
        }


    }
}
