using Insurance.Application.Geography.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.WebApi.Controllers.Geography
{
    [ApiController]
    //[Authorize(Roles = "Broker")]
    [Route("api/brokers/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CountriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetCountries(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetCountriesQuery(), cancellationToken));
        }

        [HttpGet("{countryId}/counties")]
        public async Task<IActionResult> GetCountiesFromCountry([FromRoute] Guid countryId, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetCountiesByCountryQuery(countryId), cancellationToken));
        }

        [HttpGet("{countyId}/cities")]
        public async Task<IActionResult> GetCitiesFromCounty([FromRoute] Guid countyId, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetCitiesByCountyQuery(countyId), cancellationToken));
        }

    }
}
