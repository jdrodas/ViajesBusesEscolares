using Asp.Versioning;
using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusesEscolares_PGSQL.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/buses")]
    public class BusesController(BusService busService) : Controller
    {
        private readonly BusService _busService = busService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var losBuses = await _busService
                .GetAllAsync();

                return Ok(losBuses);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

        [HttpGet("{busId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid busId)
        {
            try
            {
                var unBus = await _busService
                    .GetByIdAsync(busId);

                return Ok(unBus);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

        [HttpGet("{busId:Guid}/viajes")]
        public async Task<IActionResult> GetAssociatedTripsByIdAsync(Guid busId)
        {
            try
            {
                var losViajes = await _busService
                    .GetAssociatedTripsByIdAsync(busId);

                return Ok(losViajes);
            }
            catch (AppValidationException error)
            {
                return BadRequest(error.Message);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

    }
}
