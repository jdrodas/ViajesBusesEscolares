using Asp.Versioning;
using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Models;
using BusesEscolares_PGSQL.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusesEscolares_PGSQL.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/viajes")]
    public class ViajesController(ViajeService viajeService) : Controller
    {
        private readonly ViajeService _viajeService = viajeService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] ViajeParametrosConsulta parametrosConsulta)
        {
            try
            {
                var losViajes = await _viajeService
                .GetAllAsync(parametrosConsulta);

                return Ok(losViajes);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
            catch (AppValidationException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

        [HttpGet("{viajeId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid viajeId)
        {
            try
            {
                var unViaje = await _viajeService
                    .GetByIdAsync(viajeId);

                return Ok(unViaje);
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
