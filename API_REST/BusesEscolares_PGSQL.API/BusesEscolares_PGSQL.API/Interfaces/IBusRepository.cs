using BusesEscolares_PGSQL.API.Models;

namespace BusesEscolares_PGSQL.API.Interfaces
{
    public interface IBusRepository
    {
        public Task<List<Bus>> GetAllAsync(BusParametrosConsulta parametrosConsulta);
        public Task<int> GetTotalAsync();
        public Task<Bus> GetByIdAsync(Guid busId);
    }
}
