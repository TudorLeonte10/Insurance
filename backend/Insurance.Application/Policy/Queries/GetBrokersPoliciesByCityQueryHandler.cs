using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Queries
{
    public class GetBrokersPoliciesByCityQueryHandler : IRequestHandler<GetBrokersPoliciesByCityQuery, IEnumerable<PolicyByCityDto>>
    {
        private readonly IPolicyReadRepository _policyReadRepository;
        private readonly ICurrentUserContext _currentUserContext;
        public GetBrokersPoliciesByCityQueryHandler(IPolicyReadRepository policyReadRepository, ICurrentUserContext currentUserContext)
        {
            _policyReadRepository = policyReadRepository;
            _currentUserContext = currentUserContext;
        }
        public async Task<IEnumerable<PolicyByCityDto>> Handle(GetBrokersPoliciesByCityQuery request, CancellationToken cancellationToken)
        {
            var brokerId = _currentUserContext.BrokerId;
            return await _policyReadRepository.GetPolicyByCityAsync((Guid)brokerId!, cancellationToken);
        }
    }
}
