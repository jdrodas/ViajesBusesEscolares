using Asp.Versioning;
using BusesEscolares_NOSQL.API.Exceptions;
using BusesEscolares_NOSQL.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusesEscolares_NOSQL.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/zonas")]
    public class ZonasController(ZonaService zonaService) : Controller
    {
        private readonly ZonaService _zonaService = zonaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var lasZonas = await _zonaService
                .GetAllAsync();

                return Ok(lasZonas);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

        [HttpGet("{zonaId:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string zonaId)
        {
            try
            {
                var unaZona = await _zonaService
                    .GetByIdAsync(zonaId);

                return Ok(unaZona);
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

        [HttpGet("{zonaId:length(24)}/viajes")]
        public async Task<IActionResult> GetAssociatedTripsByIdAsync(string zonaId)
        {
            try
            {
                var losViajes = await _zonaService
                    .GetAssociatedTripsByIdAsync(zonaId);

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
