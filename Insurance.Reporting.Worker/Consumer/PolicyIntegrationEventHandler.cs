using Insurance.Application.Events;
using Insurance.Reporting.Infrastructure.Entities;
using Insurance.Reporting.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Insurance.Reporting.Worker.Consumer
{
    public class PolicyIntegrationEventHandler : IPolicyIntegrationEventHandler
    {
        private readonly ReportingDbContext _reportingDb;
        public PolicyIntegrationEventHandler(ReportingDbContext reportingDb)
        {
            _reportingDb = reportingDb;
        }
        public async Task<bool> Handle(string eventType, string message, CancellationToken cancellationToken)
        {
            switch(eventType)
            {
                case nameof(PolicyCreatedIntegrationEvent):
                    var created = JsonSerializer.Deserialize<PolicyCreatedIntegrationEvent>(message);
                    if (created == null) return false;

                    if (!await _reportingDb.PolicyReportAggregates
                        .AnyAsync(x => x.PolicyId == created.PolicyId, cancellationToken))
                    {
                        _reportingDb.PolicyReportAggregates.Add(new PolicyReportAggregate
                        {
                            PolicyId = created.PolicyId,
                            Country = created.Country,
                            County = created.County,
                            City = created.City,
                            BrokerCode = created.BrokerCode,
                            Currency = created.Currency,
                            Status = created.Status,
                            BuildingType = created.BuildingType,
                            FinalPremium = created.FinalPremium,
                            FinalPremiumInBase = created.FinalPremiumInBase,
                            StartDate = created.StartDate,
                            EndDate = created.EndDate,
                            CreatedAt = DateTime.UtcNow
                        });

                        await _reportingDb.SaveChangesAsync(cancellationToken);
                    }
                    return true;

                case nameof(PolicyStatusChangedIntegrationEvent):
                    var statusChanged = JsonSerializer.Deserialize<PolicyStatusChangedIntegrationEvent>(message);
                    if (statusChanged == null) return false;

                    var policy = await _reportingDb.PolicyReportAggregates
                        .FirstOrDefaultAsync(x => x.PolicyId == statusChanged.PolicyId, cancellationToken);

                    if (policy != null)
                    {
                        policy.Status = statusChanged.NewStatus;
                        await _reportingDb.SaveChangesAsync(cancellationToken);
                    }
                    return true;
            }
            return false;
        }
    }
}
