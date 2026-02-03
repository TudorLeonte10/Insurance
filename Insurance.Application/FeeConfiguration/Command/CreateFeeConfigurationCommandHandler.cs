using Insurance.Application.Abstractions;
using Insurance.Domain.Metadata;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.FeeConfiguration.Command
{
    public class CreateFeeConfigurationCommandHandler : IRequestHandler<CreateFeeConfigurationCommand, Guid>
    {
        private readonly IFeeConfigurationRepository _feeConfigurationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateFeeConfigurationCommandHandler(IFeeConfigurationRepository feeConfigurationRepository, IUnitOfWork unitOfWork)
        {
            _feeConfigurationRepository = feeConfigurationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateFeeConfigurationCommand request, CancellationToken cancellationToken)
        {
            var model = new Domain.Metadata.FeeConfiguration
            {
                Id = Guid.NewGuid(),
                Name = request.Dto.Name,
                Percentage = request.Dto.Percentage,
                Type = request.Dto.Type,
                EffectiveFrom = request.Dto.EffectiveFrom,
                EffectiveTo = request.Dto.EffectiveTo,
            };

            await _feeConfigurationRepository.AddAsync(model, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return model.Id;
        }
    }
}
