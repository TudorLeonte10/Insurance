using Insurance.Application.Policy.Commands;
using Insurance.Application.Policy.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/admin/policy-review")]
    public class PolicyReviewController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PolicyReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPoliciesToReview([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPoliciesToReviewQuery { PageSize = pageSize, PageNumber = pageNumber };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{policyId}/approve")]
        public async Task<IActionResult> ApprovePolicy([FromRoute] Guid policyId)
        {
            var command = new AcceptPolicyCommand(policyId);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{policyId}/reject")]
        public async Task<IActionResult> RejectPolicy([FromRoute] Guid policyId)
        {
            var command = new RejectPolicyCommand(policyId);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
