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
            return CreatedAtAction(nameof(CreatePolicy), new { policyId = id }, null);
        }

        [HttpGet("{policyId}")]
        public async Task<ActionResult<PolicyDetailsDto>> GetPolicyById(Guid policyId, CancellationToken ct = default)
        {
            var query = new GetPoliciesQuery
            {
                PolicyId = policyId,
                PageNumber = 1,
                PageSize = 1
            };

            var result = await _mediator.Send(query, ct);

            return Ok(result.Items.Single());
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
            var query = new GetPoliciesQuery
            {
                ClientId = clientId,
                BrokerId = brokerId,
                Status = status,
                StartDateFrom = startDateFrom,
                StartDateTo = startDateTo,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }
    }
}
