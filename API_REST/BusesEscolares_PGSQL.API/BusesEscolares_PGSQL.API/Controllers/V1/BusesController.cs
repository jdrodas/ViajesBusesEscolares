using Asp.Versioning;
using BusesEscolares_PGSQL.API.Exceptions;
using BusesEscolares_PGSQL.API.Models;
using BusesEscolares_PGSQL.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace BusesEscolares_PGSQL.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/buses")]
    public class BusesController(BusService busService) : Controller
    {
        private readonly BusService _busService = busService;

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] BusParametrosConsulta parametrosConsulta)
        {
            try
            {
                var losBuses = await _busService
                .GetAllAsync(parametrosConsulta);

                return Ok(losBuses);
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

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Bus unBus)
        {
            try
            {
                var busCreado = await _busService
                    .CreateAsync(unBus);

                return Ok(busCreado);
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
                var busActualizado = await _busService
                    .UpdateAsync(unBus);

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

        [HttpDelete("{busId:Guid}")]
        public async Task<IActionResult> RemoveAsync(Guid busId)
        {
            try
            {
                var placaBusBorrado = await _busService
                    .RemoveAsync(busId);

                return Ok($"El bus de placa {placaBusBorrado} fue eliminado correctamente!");
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
