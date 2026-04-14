using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Statistics.Queries
{
    public class GetPolicyTimeseriesQuery : IRequest<IEnumerable<PolicyTimeseriesDto>>
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Status { get; set; }
        public string? Currency { get; set; }
        public string? BuildingType { get; set; }
    }
}
