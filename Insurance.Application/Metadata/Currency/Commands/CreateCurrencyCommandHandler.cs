using Insurance.Application.Abstractions;
using Insurance.Domain.Metadata;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.Currency.Commands
{
    public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, Guid>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUnitOfWork _unitOfWork;   
        public CreateCurrencyCommandHandler(ICurrencyRepository currencyRepository, IUnitOfWork unitOfWork)
        {
            _currencyRepository = currencyRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var model = new Domain.Metadata.Currency
            {
                Id = Guid.NewGuid(),
                Code = request.Dto.Code,
                Name = request.Dto.Name,
                ExchangeRateToBase = request.Dto.ExchangeRateToBase,
            };

            await _currencyRepository.AddAsync(model, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return model.Id;
        }
    }
}
