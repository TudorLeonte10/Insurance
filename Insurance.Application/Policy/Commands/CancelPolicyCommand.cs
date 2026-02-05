using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Commands
{
    public record CancelPolicyCommand(Guid policyId, string? cancellationReason) : IRequest<Guid>;
    
}
