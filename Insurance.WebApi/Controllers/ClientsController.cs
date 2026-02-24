using Insurance.Application.Clients.Commands;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Clients.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Broker")]
    [ApiExplorerSettings(GroupName = "broker")]
    [Route("api/brokers/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(CreateClientDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateClientCommand(dto);
            var clientId = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetClientById), new { clientId }, null);
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientById(Guid clientId, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetClientsQuery
            {
                ClientId = clientId
            }));
        }


        [HttpPut("{clientId}")]
        public async Task<IActionResult> UpdateClient([FromRoute] Guid clientId, [FromBody] UpdateClientDto dto, CancellationToken ct)
        {
            var command = new UpdateClientCommand(clientId, dto);
            await _mediator.Send(command, ct);
            return NoContent();
        }

        [HttpGet("clients")]
        public async Task<IActionResult> SearchClients([FromQuery] string? name, [FromQuery] string? identifier, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _mediator.Send(new GetClientsQuery
            {
                Name = name,
                IdentificationNumber = identifier,
                PageNumber = pageNumber,
                PageSize = pageSize
            }));
        }
    }
}

