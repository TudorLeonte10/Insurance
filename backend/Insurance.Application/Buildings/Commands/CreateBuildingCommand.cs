using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Buildings.Commands
{
    public record CreateBuildingCommand(Guid ClientId, CreateBuildingDto BuildingDto) : IRequest<Guid>, IRequireBrokerValidation;
}
