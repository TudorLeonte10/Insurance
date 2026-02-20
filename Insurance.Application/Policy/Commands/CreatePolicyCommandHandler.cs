using Insurance.Application.Abstractions;
using Insurance.Application.Abstractions.Messaging;
using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Events;
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
        private readonly IIntegrationEventPublisher _eventPublisher;

        public CreatePolicyCommandHandler(
            IPolicyCreationService policyCreationService,
            IPolicyRepository policyRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserContext currentUserContext,
            IIntegrationEventPublisher eventPublisher)
        {
            _policyCreationService = policyCreationService;
            _policyRepository = policyRepository;
            _unitOfWork = unitOfWork;
            _currentUserContext = currentUserContext;
            _eventPublisher = eventPublisher;
        }

        public async Task<Guid> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            var brokerId = _currentUserContext.BrokerId!.Value;

            var policy = await _policyCreationService.CreatePolicyAsync(request.PolicyDto, brokerId, cancellationToken);

        
            await _policyRepository.AddAsync(policy.Policy, cancellationToken);

            var integrationEvent = new PolicyCreatedIntegrationEvent(
                policy.Policy.Id,
                policy.Country,
                policy.County,
                policy.City,
                policy.BrokerCode,
                policy.Currency,
                policy.Status,
                policy.BuildingType,
                policy.FinalPremium,
                policy.FinalPremiumInBase,
                policy.Policy.StartDate,
                policy.Policy.EndDate,
                policy.Policy.CreatedAt
   );

            await _eventPublisher.Publish(integrationEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Policy.Id;
        }


    }
}
