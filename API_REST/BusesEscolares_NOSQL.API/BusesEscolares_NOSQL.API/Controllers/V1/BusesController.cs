using Asp.Versioning;
using BusesEscolares_NOSQL.API.Exceptions;
using BusesEscolares_NOSQL.API.Models;
using BusesEscolares_NOSQL.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

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
                var unColor = await _busService
                    .GetByIdAsync(busId);

                return Ok(unColor);
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