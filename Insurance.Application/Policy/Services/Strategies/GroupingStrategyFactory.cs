using Insurance.Application.Exceptions;
using Insurance.Application.Policy.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services.Strategies
{
    public class GroupingStrategyFactory : IGroupingStrategyFactory
    {
        private readonly IEnumerable<IReportGroupingStrategy> _strategies;
        public GroupingStrategyFactory(IEnumerable<IReportGroupingStrategy> strategies)
        {
            _strategies = strategies;
        }
        public IReportGroupingStrategy Create(ReportGroupingType groupingType)
        {
            var strategy = _strategies.FirstOrDefault(s => s.GroupingType == groupingType);
            return strategy ?? throw new NotFoundException("Strategy not found");
        }
    }
}
