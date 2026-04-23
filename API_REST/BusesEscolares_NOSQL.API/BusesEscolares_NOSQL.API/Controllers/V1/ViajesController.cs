using Asp.Versioning;
using BusesEscolares_NOSQL.API.Exceptions;
using BusesEscolares_NOSQL.API.Models;
using BusesEscolares_NOSQL.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusesEscolares_NOSQL.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/viajes")]
    public class ViajesController(ViajeService viajeService) : Controller
    {
        private readonly ViajeService _viajeService = viajeService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var losViajes = await _viajeService
                .GetAllAsync();

                return Ok(losViajes);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }

        [HttpGet("{viajeId:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string viajeId)
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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Viaje unViaje)
        {
            try
            {
                var viajeCreado = await _viajeService
                    .CreateAsync(unViaje);

                return Ok(viajeCreado);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }
    }
}
