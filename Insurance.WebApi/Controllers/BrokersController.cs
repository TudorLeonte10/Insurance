using Insurance.Application.Brokers.Commands;
using Insurance.Application.Brokers.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [ApiController]
    public class BrokersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BrokersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/admin/brokers")]
        public async Task<IActionResult> Create([FromBody] CreateBrokerDto dto)
        {
            var command = new CreateBrokerCommand(dto);
            var id = await _mediator.Send(command);

            return Ok(id);
        }

    }
}
