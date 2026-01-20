using Insurance.Application.Buildings.Commands;
using Insurance.Application.Buildings.DTOs;
using Insurance.Application.Buildings.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [ApiController]
    [Route("api/brokers/")]
    public class BuildingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BuildingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("clients/{clientId}/buildings")]
        public async Task<IActionResult> GetBuildingsByClient([FromRoute] Guid clientId, CancellationToken cancellationToken)
        {
            var query = new GetBuildingsByClientQuery(clientId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("buildings/{buildingId}")]
        public async Task<IActionResult> GetBuildingById([FromRoute] Guid buildingId, CancellationToken cancellationToken)
        {
            var query = new GetBuildingByIdQuery(buildingId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("clients/{clientId}/buildings")]
        public async Task<IActionResult> CreateBuilding([FromRoute] Guid clientId, [FromBody] CreateBuildingDto buildingDto, CancellationToken cancellationToken)
        {
            var command = new CreateBuildingCommand(clientId, buildingDto);
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetBuildingById), new { buildingId = result }, null);
        }

        [HttpPut("buildings/{buildingId}")]
        public async Task<IActionResult> UpdateBuilding([FromRoute] Guid buildingId, [FromBody] UpdateBuildingDto buildingDto, CancellationToken cancellationToken)
        {
            var command = new UpdateBuildingCommand(buildingId, buildingDto);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
