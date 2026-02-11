using Insurance.Application.Metadata.Currency.Commands;
using Insurance.Application.Metadata.Currency.DTOs;
using Insurance.Application.Metadata.Currency.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers.Metadata
{
    [ApiController]
    //[Authorize(Roles = "Admin")]
    [Route("api/admin/currencies")]
    public class CurrencyController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CurrencyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetCurrenciesQuery(pageNumber, pageSize);
            var currencies = await _mediator.Send(query, cancellationToken);
            return Ok(currencies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCurrency([FromBody] CreateCurrencyDto dto,  CancellationToken cancellationToken)
        {
            var command = new CreateCurrencyCommand(dto);
            var createdCurrency = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { id = createdCurrency }, null);
        }

        [HttpPut("{currencyId}")]
        public async Task<IActionResult> UpdateCurrency(Guid currencyId, [FromBody] UpdateCurrencyDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateCurrencyCommand(dto, currencyId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
