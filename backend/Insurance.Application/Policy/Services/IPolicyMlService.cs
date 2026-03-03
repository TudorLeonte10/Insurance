using Insurance.Application.Policy.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public interface IPolicyMlService
    {
        Task<PolicyMlResult> AnalyzePolicyAsync(AnomalyFeatureDto featureDto, CancellationToken cancellationToken);
    }
}
