using Insurance.Application.Metadata.RiskFactors.Commands;
using Insurance.Application.Metadata.RiskFactors.DTOs;
using Insurance.Application.Metadata.RiskFactors.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers.Metadata
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/admin/risk-factors")]
    public class RiskFactorConfigurationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RiskFactorConfigurationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRiskFactors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllRiskFactorsQuery(pageNumber, pageSize);
            var riskFactors = await _mediator.Send(query);
            return Ok(riskFactors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRiskFactor([FromBody] CreateRiskFactorConfigurationDto dto, CancellationToken cancellationToken)
        {
            var command = new CreateRiskFactorConfigurationCommand(dto);
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAllRiskFactors), new { id = result }, null);
        }

        [HttpPut("{riskFactorId}")]
        public async Task<IActionResult> UpdateRiskFactor(Guid riskFactorId, [FromBody] UpdateRiskFactorConfigurationDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateRiskFactorConfigurationCommand(dto, riskFactorId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
