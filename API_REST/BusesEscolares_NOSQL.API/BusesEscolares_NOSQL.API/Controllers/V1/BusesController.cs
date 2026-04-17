using Asp.Versioning;
using BusesEscolares_NOSQL.API.Exceptions;
using BusesEscolares_NOSQL.API.Models;
using BusesEscolares_NOSQL.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusesEscolares_NOSQL.API.Controllers.V1
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

        [HttpGet("{busId:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string busId)
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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Bus unBus)
        {
            try
            {
                var colorCreado = await _busService
                    .CreateAsync(unBus);

                return Ok(colorCreado);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Bus unBus)
        {
            try
            {
                var colorActualizado = await _busService
                    .UpdateAsync(unBus);

                return Ok(colorActualizado);
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

        [HttpDelete("{colorId:length(24)}")]
        public async Task<IActionResult> RemoveAsync(string colorId)
        {
            try
            {
                var nombreColorBorrado = await _busService
                    .RemoveAsync(colorId);

                return Ok($"El color {nombreColorBorrado} fue eliminado correctamente!");
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