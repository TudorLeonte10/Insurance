using Insurance.Application.Metadata.Currency.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Metadata.Currency.Commands
{
    public record CreateCurrencyCommand(CreateCurrencyDto Dto) : IRequest<Guid>;
}
