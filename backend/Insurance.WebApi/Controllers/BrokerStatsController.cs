using Insurance.Application.Policy.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Broker")]
    [ApiExplorerSettings(GroupName = "broker")]
    [Route("api/brokers/stats")]
    public class BrokerStatsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BrokerStatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("policy-by-city")]
        public async Task<IActionResult> GetPoliciesByCity(CancellationToken cancellationToken)
        {
            var query = new GetBrokersPoliciesByCityQuery();
            var stats = await _mediator.Send(query, cancellationToken);
            return Ok(stats);
        }
    }
}
