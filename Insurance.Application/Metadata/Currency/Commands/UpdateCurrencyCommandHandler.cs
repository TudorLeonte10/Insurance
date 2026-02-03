using Insurance.Application.Abstractions;
using Insurance.Application.Exceptions;
using Insurance.Domain.Metadata;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.Currency.Commands
{
    public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, Guid>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCurrencyCommandHandler(ICurrencyRepository currencyRepository, IUnitOfWork unitOfWork)
        {
            _currencyRepository = currencyRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = await _currencyRepository.GetByIdAsync(request.Id, cancellationToken);

            if (currency == null)
            {
                throw new NotFoundException($"Currency with Id {request.Id} not found.");
            }

            currency.Code = request.Dto.Code;
            currency.Name = request.Dto.Name;
            currency.ExchangeRateToBase = request.Dto.ExchangeRateToBase;

            await _currencyRepository.UpdateAsync(currency, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return currency.Id;

        }
    }
}
