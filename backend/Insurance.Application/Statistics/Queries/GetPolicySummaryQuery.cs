using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Statistics.Queries
{
    public class GetPolicySummaryQuery() : IRequest<PolicySummaryDto>
    {
        public DateTime? From { get; init; }
        public DateTime? To { get; init; }
        public string? Status { get; init; }
        public string? Currency { get; init; }
        public string? BuildingType { get; init; }
    }
}
