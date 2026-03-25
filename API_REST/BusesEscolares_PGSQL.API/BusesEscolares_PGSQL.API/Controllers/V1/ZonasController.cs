using Asp.Versioning;
using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Services;
using BusesEscolares_PGSQL.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusesEscolares_PGSQL.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/zonas")]
    public class ZonasController(ZonaService zonaService) : Controller
    {
        private readonly ZonaService _zonaService = zonaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] ZonaParametrosConsulta parametrosConsulta)
        {
            try
            {
                var lasZonas = await _zonaService
                .GetAllAsync(parametrosConsulta);

                return Ok(lasZonas);
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

        [HttpGet("{zonaId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid zonaId)
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

        [HttpGet("{zonaId:Guid}/viajes")]
        public async Task<IActionResult> GetAssociatedTripsByIdAsync(Guid zonaId)
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
