using Insurance.Application.Brokers.Commands;
using Insurance.Application.Brokers.DTOs;
using Insurance.Application.Brokers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [ApiController]
    [Route("api/admin/brokers")]
    public class BrokersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BrokersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetBrokersQuery(pageNumber, pageSize);
            var brokers = await _mediator.Send(query);
            return Ok(brokers);
        }

        [HttpGet("{brokerId}")]
        public async Task<IActionResult> GetById(Guid brokerId)
        {
            var query = new GetBrokerByIdQuery(brokerId);
            var broker = await _mediator.Send(query);
            return broker is not null ? Ok(broker) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrokerDto dto)
        {
            var command = new CreateBrokerCommand(dto);
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { brokerId = id }, null);
        }

        [HttpPut("{brokerId}")]
        public async Task<IActionResult> Update(Guid brokerId, [FromBody] UpdateBrokerDto dto)
        {
            var command = new UpdateBrokerCommand(dto, brokerId);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{brokerId}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid brokerId)
        {
            var command = new ChangeBrokerStatusCommand(brokerId, false);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{brokerId}/activate")]
        public async Task<IActionResult> Activate(Guid brokerId)
        {
            var command = new ChangeBrokerStatusCommand(brokerId, true);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
