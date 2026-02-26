using Insurance.Application.Common;
using Insurance.Application.Common.Paging;
using Insurance.Application.Policy.DTOs;
using Insurance.Domain.Policies;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Queries
{
   public class SearchPoliciesQuery : IRequest<PagedResult<PolicyDetailsDto>>, IRequireBrokerValidation
{
    public Guid? ClientId { get; init; }
    public PolicyStatus? Status { get; init; }

    public DateTime? StartDateFrom { get; init; }
    public DateTime? StartDateTo { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}


}
