using Insurance.Application.Abstractions;
using Insurance.Application.Exceptions;
using Insurance.Domain.Metadata;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.RiskFactors.Commands
{
    public class UpdateRiskFactorConfigurationCommandHandler : IRequestHandler<UpdateRiskFactorConfigurationCommand, Guid>
    {
        private readonly IRiskFactorConfigurationRepository _riskFactorConfigurationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRiskFactorConfigurationCommandHandler(
            IRiskFactorConfigurationRepository riskFactorConfigurationRepository,
            IUnitOfWork unitOfWork)
        {
            _riskFactorConfigurationRepository = riskFactorConfigurationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(UpdateRiskFactorConfigurationCommand request, CancellationToken cancellationToken)
        {
            var existingConfig = await _riskFactorConfigurationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existingConfig == null)
            {
                throw new NotFoundException($"RiskFactorConfiguration with id {request.Id} not found");
            }

            existingConfig.Level = request.Dto.Level;
            existingConfig.AdjustmentPercentage = request.Dto.AdjustmentPercentage;
            existingConfig.ReferenceId = request.Dto.ReferenceId;

            await _riskFactorConfigurationRepository.UpdateAsync(existingConfig, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return existingConfig.Id;
        }
    }
}
