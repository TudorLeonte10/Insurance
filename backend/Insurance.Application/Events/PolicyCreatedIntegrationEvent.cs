using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Events
{
    public record PolicyCreatedIntegrationEvent(
        Guid PolicyId,
        string Country,
        string County,
        string City,
        string BrokerCode,
        string Currency,
        string Status,
        string BuildingType,
        decimal FinalPremium,
        decimal FinalPremiumInBase,
        DateTime StartDate,
        DateTime EndDate,
        DateTime CreatedAt);
}
