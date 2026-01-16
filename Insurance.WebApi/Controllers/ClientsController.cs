using Insurance.Application.Clients.Commands;
using Insurance.Application.Clients.DTOs;
using Insurance.Application.Clients.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [ApiController]
    [Route("api/broker/[controller]")]
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

            return Ok(new { id = clientId });
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClientById(Guid clientId, CancellationToken cancellationToken)
        {
            var query = new GetClientByIdQuery(clientId);
            var client = await _mediator.Send(query, cancellationToken);
            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetClients([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetClientsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{clientId}")]
        public async Task<IActionResult> UpdateClient([FromRoute] Guid clientId, [FromBody] UpdateClientDto dto, CancellationToken ct)
        {
            var command = new UpdateClientCommand(clientId, dto);
            await _mediator.Send(command, ct);
            return NoContent();
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClient([FromRoute] Guid clientId, CancellationToken ct)
        {
            var command = new DeleteClientCommand(clientId);
            await _mediator.Send(command, ct);
            return NoContent();
        }
    }
}
