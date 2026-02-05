using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Commands
{
    public record ActivatePolicyCommand(Guid policyId) : IRequest<Guid>;
}
