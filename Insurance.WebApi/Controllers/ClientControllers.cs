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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetClientByIdQuery(id);
            var client = await _mediator.Send(query, cancellationToken);
            return Ok(client);
        }

        [HttpGet]
        public async Task<IActionResult> GetClients([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(
                new GetClientsQuery(pageNumber, pageSize));

            return Ok(result);
        }

    }
}
