using Insurance.Application.Buildings.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers
{
    [ApiController]
    [Route("api/brokers/clients/")]
    public class BuildingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BuildingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{clientId}/buildings")]
        public async Task<IActionResult> GetBuildingsByClient([FromRoute] Guid clientId, CancellationToken cancellationToken)
        {
            var query = new GetBuildingsByClientQuery(clientId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
