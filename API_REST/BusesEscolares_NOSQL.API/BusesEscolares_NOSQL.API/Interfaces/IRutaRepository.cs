using BusesEscolares_NOSQL.API.Models;

namespace BusesEscolares_NOSQL.API.Interfaces
{
    public interface IRutaRepository
    {
        public Task<List<Ruta>> GetAllAsync();
        public Task<long> GetTotalAsync();
        public Task<Ruta> GetByIdAsync(string rutaId);
        public Task<Ruta> GetByDetailsAsync(Ruta unaRuta);
        public Task<bool> CreateAsync(Ruta unaRuta);
        public Task<bool> UpdateAsync(Ruta unaRuta);
        public Task<bool> RemoveAsync(string rutaId);
    }
}
