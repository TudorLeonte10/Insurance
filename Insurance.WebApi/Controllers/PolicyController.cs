using Insurance.Application.Common.Paging;
using Insurance.Application.Policy.Commands;
using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Queries;
using Insurance.Domain.Policies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [ApiController]
    [Route("api/brokers/policies")]
    public class PolicyController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PolicyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePolicy([FromBody] CreatePolicyDto dto)
        {
            var command = new CreatePolicyCommand(dto);
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPolicyById), new { policyId = id }, null);
        }

        [HttpPost("{policyId}/activate")]
        public async Task<IActionResult> ActivatePolicy([FromRoute] Guid policyId)
        {
            var command = new ActivatePolicyCommand(policyId);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("{policyId}/cancel")]
        public async Task<IActionResult> CancelPolicy([FromRoute] Guid policyId, [FromBody] string? cancellationReason)
        {
            var command = new CancelPolicyCommand(policyId, cancellationReason);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{policyId}")]
        public async Task<ActionResult<PolicyDetailsDto>> GetPolicyById(Guid policyId, CancellationToken ct)
        {
            var policy = await _mediator.Send(
                new GetPolicyByIdQuery(policyId), ct);

            return Ok(policy);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<PolicyDetailsDto>>> GetPolicies(
     [FromQuery] Guid? clientId,
     [FromQuery] Guid? brokerId,
     [FromQuery] PolicyStatus? status,
     [FromQuery] DateTime? startDateFrom,
     [FromQuery] DateTime? startDateTo,
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10,
     CancellationToken ct = default)
        {
            var result = await _mediator.Send(
                new SearchPoliciesQuery
                {
                    ClientId = clientId,
                    BrokerId = brokerId,
                    Status = status,
                    StartDateFrom = startDateFrom,
                    StartDateTo = startDateTo,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                }, ct);

            return Ok(result);
        }
    }
}
