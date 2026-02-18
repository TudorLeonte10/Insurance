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
    [Route("api/admin/reports")]
    public class PolicyReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PolicyReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("policies-by-country")]
        public async Task<IActionResult> GetReportByCountry(
            [FromQuery] GetPoliciesReportRequestDto request,
            CancellationToken cancellationToken
            )
        {
            var query = new GetPoliciesReportQuery(request, ReportGroupingType.Country);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("policies-by-county")]
        public async Task<IActionResult> GetReportByCounty(
            [FromQuery] GetPoliciesReportRequestDto request,
            CancellationToken cancellationToken
            )
        {
            var query = new GetPoliciesReportQuery(request, ReportGroupingType.County);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("policies-by-city")]
        public async Task<IActionResult> GetReportByCity(
            [FromQuery] GetPoliciesReportRequestDto request,
            CancellationToken cancellationToken
            )
        {
            var query = new GetPoliciesReportQuery(request, ReportGroupingType.City);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("policies-by-broker")]
        public async Task<IActionResult> GetReportByBroker(
            [FromQuery] GetPoliciesReportRequestDto request,
            CancellationToken cancellationToken
            )
        {
            var queryWithGrouping = new GetPoliciesReportQuery(request, ReportGroupingType.Broker);
            var result = await _mediator.Send(queryWithGrouping, cancellationToken);
            return Ok(result);
        }
    }
}
