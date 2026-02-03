using Insurance.Application.FeeConfiguration.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.FeeConfiguration.Command
{
   public record CreateFeeConfigurationCommand(CreateFeeConfigurationDto Dto) : IRequest<Guid>;
   
}
