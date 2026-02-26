using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Common.Paging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Brokers.Queries
{
    public class GetBrokersQueryHandler : IRequestHandler<GetBrokersQuery, PagedResult<BrokerDetailsDto>>
    {
        private readonly IBrokerReadRepository _brokerReadRepository;

        public GetBrokersQueryHandler(IBrokerReadRepository brokerReadRepository)
        {
            _brokerReadRepository = brokerReadRepository;
        }

        public async Task<PagedResult<BrokerDetailsDto>> Handle(GetBrokersQuery request, CancellationToken cancellationToken)
        {
            var brokers = await _brokerReadRepository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            return brokers;
        }
    }
}
