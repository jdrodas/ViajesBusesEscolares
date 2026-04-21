using BusesEscolares_NOSQL.API.Models;

namespace BusesEscolares_NOSQL.API.Interfaces
{
    public interface IZonaRepository
    {
        public Task<List<Zona>> GetAllAsync();
        public Task<long> GetTotalAsync();
        public Task<Zona> GetByIdAsync(string zonaId);
        public Task<Zona> GetByNameAsync(string zonaNombre);
    }
}
