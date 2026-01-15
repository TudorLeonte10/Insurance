using Insurance.Application.Clients.Commands;
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
        public async Task<IActionResult> CreateClient(CreateClientCommand command, CancellationToken cancellationToken)
        {
            var clientId = await _mediator.Send(command);
            return Ok(new { id = clientId });
        }

    }
}
