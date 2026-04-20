using Asp.Versioning;
using BusesEscolares_NOSQL.API.Exceptions;
using BusesEscolares_NOSQL.API.Models;
using BusesEscolares_NOSQL.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusesEscolares_NOSQL.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/rutas")]
    public class RutasController(RutaService rutaService) : Controller
    {
        private readonly RutaService _rutaService = rutaService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var lasRutas = await _rutaService
                .GetAllAsync();

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

        [HttpGet("{rutaId:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string rutaId)
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

        [HttpGet("{rutaId:length(24)}/viajes")]
        public async Task<IActionResult> GetAssociatedTripsByIdAsync(string rutaId)
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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Ruta unaRuta)
        {
            try
            {
                var rutaCreada = await _rutaService
                    .CreateAsync(unaRuta);

                return Ok(rutaCreada);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Ruta unaRuta)
        {
            try
            {
                var busActualizado = await _rutaService
                    .UpdateAsync(unaRuta);

                return Ok(busActualizado);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpDelete("{rutaId:length(24)}")]
        public async Task<IActionResult> RemoveAsync(string rutaId)
        {
            try
            {
                var nombreRutaBorrada = await _rutaService
                    .RemoveAsync(rutaId);

                return Ok($"La ruta {nombreRutaBorrada} fue eliminada correctamente!");
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }
    }
}
