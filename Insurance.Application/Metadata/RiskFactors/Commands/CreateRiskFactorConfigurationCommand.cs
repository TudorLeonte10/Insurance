using Insurance.Application.Metadata.RiskFactors.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.RiskFactors.Commands
{
    public record CreateRiskFactorConfigurationCommand(CreateRiskFactorConfigurationDto RiskFactorConfigurationDto) : IRequest<Guid>;
}
