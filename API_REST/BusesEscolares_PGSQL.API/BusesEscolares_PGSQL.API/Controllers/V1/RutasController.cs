using Asp.Versioning;
using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Models;
using BusesEscolares_PGSQL.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusesEscolares_PGSQL.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/rutas")]
    public class RutasController(RutaService rutaService) : Controller
    {
        private readonly RutaService _rutaService = rutaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] RutaParametrosConsulta parametrosConsulta)
        {
            try
            {
                var lasRutas = await _rutaService
                .GetAllAsync(parametrosConsulta);

                return Ok(lasRutas);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
        }

        [HttpGet("{rutaId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid rutaId)
        {
            try
            {
                var unaRuta = await _rutaService
                    .GetByIdAsync(rutaId);

                return Ok(unaRuta);
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

        [HttpGet("{rutaId:Guid}/viajes")]
        public async Task<IActionResult> GetAssociatedTripsByIdAsync(Guid rutaId)
        {
            try
            {
                var losViajes = await _rutaService
                    .GetAssociatedTripsByIdAsync(rutaId);

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
