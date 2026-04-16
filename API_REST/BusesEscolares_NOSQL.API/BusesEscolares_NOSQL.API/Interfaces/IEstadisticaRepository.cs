using BusesEscolares_NOSQL.API.Models;

namespace BusesEscolares_NOSQL.API.Repositories
{
    public interface IEstadisticaRepository
    {
        public Task<Estadistica> GetAllAsync();
    }
}