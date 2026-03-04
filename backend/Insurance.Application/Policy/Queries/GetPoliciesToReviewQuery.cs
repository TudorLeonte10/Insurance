using Insurance.Application.Common.Paging;
using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Queries
{
    public class GetPoliciesToReviewQuery : IRequest<PagedResult<PolicyDetailsDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
       
    }
}
