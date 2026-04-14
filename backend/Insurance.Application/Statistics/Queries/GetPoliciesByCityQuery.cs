using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Statistics.Queries
{
    public class GetPoliciesByCityReportQuery : IRequest<IEnumerable<PolicyByCityDto>>
    {
    }
}
