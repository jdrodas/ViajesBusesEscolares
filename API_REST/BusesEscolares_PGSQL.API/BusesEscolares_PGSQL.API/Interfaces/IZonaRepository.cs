using BusesEscolares_PGSQL.API.Models;

namespace BusesEscolares_PGSQL.API.Interfaces
{
    public interface IZonaRepository
    {
        public Task<List<Zona>> GetAllAsync(ZonaParametrosConsulta parametrosConsulta);

        public Task<int> GetTotalAsync();
        public Task<Zona> GetByIdAsync(Guid zonaId);
    }
}
