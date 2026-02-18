
using Insurance.Application.Metadata.FeeConfiguration.Command;
using Insurance.Application.Metadata.FeeConfiguration.DTOs;
using Insurance.Application.Metadata.FeeConfiguration.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers.Metadata
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ApiExplorerSettings(GroupName = "admin")]  
    [Route("api/admin/fees")]
    public class FeeConfigurationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FeeConfigurationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetFeeConfigurationQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeeConfiguration([FromBody] CreateFeeConfigurationDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateFeeConfigurationCommand(dto);
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { feeConfigurationId = id }, null);
        }

        [HttpPut("{feeConfigurationId}")]
        public async Task<IActionResult> UpdateFeeConfiguration(Guid feeConfigurationId, [FromBody] UpdateFeeConfigurationDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateFeeConfigurationCommand(feeConfigurationId, dto);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

    }
}
