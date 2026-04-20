using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;

namespace BusesEscolares_NOSQL.API.Services
{
    public class EstadisticaService(IEstadisticaRepository estadisticaRepository)
    {
        private readonly IEstadisticaRepository _estadisticaRepository = estadisticaRepository;

        public async Task<Estadistica> GetAllAsync()
        {
            return await _estadisticaRepository
            .GetAllAsync();
        }
    }
}