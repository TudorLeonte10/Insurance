using Insurance.Application.Policy.Commands;
using Insurance.Application.Policy.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Services
{
    public interface IPolicyCreationService
    {
        Task<PolicyCreationResult> CreatePolicyAsync(CreatePolicyDto dto, Guid brokerId, CancellationToken cancellationToken);
    }
}
