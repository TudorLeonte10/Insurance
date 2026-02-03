using Insurance.Application.FeeConfiguration.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.FeeConfiguration.Command
{
    public record UpdateFeeConfigurationCommand(Guid Id, UpdateFeeConfigurationDto Dto) : IRequest<Guid>;
}
