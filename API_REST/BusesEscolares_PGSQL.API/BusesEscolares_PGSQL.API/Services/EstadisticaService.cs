using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;

namespace BusesEscolares_PGSQL.API.Services
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



