using Insurance.Application.Policy.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Policy.Commands
{
    public record CreatePolicyCommand(CreatePolicyDto PolicyDto) : IRequest<Guid>;

}
