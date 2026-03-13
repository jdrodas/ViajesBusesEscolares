using BusesEscolares_PGSQL.API.Models;

namespace BusesEscolares_PGSQL.API.Interfaces
{
    public interface IEstadisticaRepository
    {
        public Task<Estadistica> GetAllAsync();
    }
}