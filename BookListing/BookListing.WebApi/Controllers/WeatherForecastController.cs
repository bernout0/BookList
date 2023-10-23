using BookListing.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookListing.Web.Controllers
{
    /// <summary>
    /// Default boilerplate endpoint for initial controller. Please disregard
    /// </summary>
    /// 
    [AllowAnonymous]
    public class WeatherForecastController : ApiControllerBase
    {

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return await Mediator.Send(new GetWeatherForecastsQuery());
        }
    }

}
