using Insurance.Application.Policy.DTOs;
using Insurance.Application.Policy.Enums;
using Insurance.Application.Policy.Queries;
using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/admin/policies/reports")]
    public class PolicyReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PolicyReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetReport(
            [FromQuery] ReportGroupingType reportGroupingType,
            [FromQuery] GetPoliciesReportRequestDto request,
            CancellationToken cancellationToken
            )
        {
            var query = new GetPoliciesReportQuery(request, reportGroupingType);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

    }
}
