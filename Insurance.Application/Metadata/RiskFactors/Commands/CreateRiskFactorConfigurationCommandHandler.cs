using Insurance.Application.Abstractions;
using Insurance.Domain.Metadata;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.RiskFactors.Commands
{
    public class CreateRiskFactorConfigurationCommandHandler : IRequestHandler<CreateRiskFactorConfigurationCommand, Guid>
    {
        private readonly IRiskFactorConfigurationRepository _riskFactorConfigurationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateRiskFactorConfigurationCommandHandler(IRiskFactorConfigurationRepository riskFactorConfigurationRepository, IUnitOfWork unitOfWork)
        {
            _riskFactorConfigurationRepository = riskFactorConfigurationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateRiskFactorConfigurationCommand request, CancellationToken cancellationToken)
        {
            var model = new RiskFactorConfiguration
            {
                Id = Guid.NewGuid(),
                Level = request.RiskFactorConfigurationDto.Level,
                AdjustmentPercentage = request.RiskFactorConfigurationDto.AdjustmentPercentage,
                ReferenceId = request.RiskFactorConfigurationDto.ReferenceId
            };

            await _riskFactorConfigurationRepository.AddAsync(model, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return model.Id;
        }
    }
}
