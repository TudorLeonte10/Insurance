using Insurance.Application.Metadata.Currency.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.Currency.Commands
{
    public record UpdateCurrencyCommand(UpdateCurrencyDto Dto, Guid Id) : IRequest<Guid>;

}
