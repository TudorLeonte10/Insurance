using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Commands
{
    public record AcceptPolicyCommand(Guid PolicyId) : IRequest<Guid>;
    
}
