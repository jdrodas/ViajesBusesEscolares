using BusesEscolares_PGSQL.API.Models;

namespace BusesEscolares_PGSQL.API.Interfaces
{
    public interface IRutaRepository
    {
        public Task<List<Ruta>> GetAllAsync();

        public Task<Ruta> GetByIdAsync(Guid rutaId);
    }
}
