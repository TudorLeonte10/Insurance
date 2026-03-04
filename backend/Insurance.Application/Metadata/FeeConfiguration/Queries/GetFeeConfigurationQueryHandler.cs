using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Common.Paging;
using Insurance.Application.Metadata.FeeConfiguration.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.FeeConfiguration.Queries
{
    public class GetFeeConfigurationQueryHandler : IRequestHandler<GetFeeConfigurationQuery, PagedResult<FeeConfigurationDto>>
    {
        private IFeeConfigurationReadRepository readRepository;
        public GetFeeConfigurationQueryHandler(IFeeConfigurationReadRepository readRepository)
        {
            this.readRepository = readRepository;
        }
        public async Task<PagedResult<FeeConfigurationDto>> Handle(GetFeeConfigurationQuery request, CancellationToken cancellationToken)
        {
            var result = await readRepository.GetPagedAsync(request.pageNumber, request.pageSize, cancellationToken);
            return result;
        }
    }
}
