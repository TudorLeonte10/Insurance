using Insurance.Application.Common.Paging;
using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Policies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Abstractions.Repositories
{
    public interface IPolicySearchRepository
    {
        Task<PagedResult<PolicyDetailsDto>> SearchAsync(
            Guid? clientId,
            Guid? brokerId,
            PolicyStatus? status,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            int pageNumber,
            int pageSize);
    }

}
