using Insurance.Application.Abstractions;
using Insurance.Application.Exceptions;
using Insurance.Domain.Metadata;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.FeeConfiguration.Command
{
    public class UpdateFeeConfigurationCommandHandler : IRequestHandler<UpdateFeeConfigurationCommand, Guid>
    {
        private readonly IFeeConfigurationRepository _feeConfigurationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateFeeConfigurationCommandHandler(IFeeConfigurationRepository feeConfigurationRepository, IUnitOfWork unitOfWork)
        {
            _feeConfigurationRepository = feeConfigurationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(UpdateFeeConfigurationCommand request, CancellationToken cancellationToken)
        {
            var feeConfiguration = await _feeConfigurationRepository.GetByIdAsync(request.Id, cancellationToken);

            if (feeConfiguration == null)
            {
                throw new NotFoundException($"Fee Configuration with Id {request.Id} not found.");
            }

            feeConfiguration.Name = request.Dto.Name;
            feeConfiguration.Type = request.Dto.Type;
            feeConfiguration.Percentage = request.Dto.Percentage;
            feeConfiguration.EffectiveFrom = request.Dto.EffectiveFrom;
            feeConfiguration.EffectiveTo = request.Dto.EffectiveTo;
            feeConfiguration.IsActive = request.Dto.IsActive;

            await _feeConfigurationRepository.UpdateAsync(feeConfiguration, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return feeConfiguration.Id;
        }
    }
}
