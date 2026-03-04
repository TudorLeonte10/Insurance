using Insurance.Application.Policy.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public class PolicyMlService : IPolicyMlService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PolicyMlService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<PolicyMlResult> AnalyzePolicyAsync(AnomalyFeatureDto featureDto, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("/score", featureDto, cancellationToken);
            var content  = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PolicyMlResult>(cancellationToken: cancellationToken);
                return result!;
            }
            else
            {
   
                throw new Exception($"Failed to analyze policy. Status code: {response.StatusCode}, Body: {content}");

            }
        }
    }
}
