using Insurance.Application.Metadata.FeeConfiguration.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.FeeConfiguration.Command
{
    public record UpdateFeeConfigurationCommand(Guid Id, UpdateFeeConfigurationDto Dto) : IRequest<Guid>;
}
