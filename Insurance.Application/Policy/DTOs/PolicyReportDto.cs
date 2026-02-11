using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.DTOs
{
    public record PolicyReportDto(
    string GroupName,
    string Currency,
    int PolicyCount,
    decimal TotalPremium,
    decimal TotalPremiumInBaseCurrency
);
}
