using Insurance.Application.Policy.Enums;
using Insurance.Application.Policy.Queries;
using Insurance.Domain.Buildings;
using Insurance.Domain.Policies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi
{
    [ApiController]
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
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] PolicyStatus? status,
            [FromQuery] string? currency,
            [FromQuery] BuildingType? buildingType,
            CancellationToken cancellationToken
            )
        {
            var query = new GetPoliciesReportQuery(ReportGroupingType.Country, from, to, status, currency, buildingType);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("policies-by-county")]
        public async Task<IActionResult> GetReportByCounty(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] PolicyStatus? status,
            [FromQuery] string? currency,
            [FromQuery] BuildingType? buildingType,
            CancellationToken cancellationToken
            )
        {
            var query = new GetPoliciesReportQuery(ReportGroupingType.County, from, to, status, currency, buildingType);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("policies-by-city")]
        public async Task<IActionResult> GetReportByCity(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] PolicyStatus? status,
            [FromQuery] string? currency,
            [FromQuery] BuildingType? buildingType,
            CancellationToken cancellationToken
            )
        {
            var query = new GetPoliciesReportQuery(ReportGroupingType.City, from, to, status, currency, buildingType);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("policies-by-broker")]
        public async Task<IActionResult> GetReportByBroker(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] PolicyStatus? status,
            [FromQuery] string? currency,
            [FromQuery] BuildingType? buildingType,
            CancellationToken cancellationToken
            )
        {
            var query = new GetPoliciesReportQuery(ReportGroupingType.Broker, from, to, status, currency, buildingType);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
