using BusesEscolares_PGSQL.API.Models;

namespace BusesEscolares_PGSQL.API.Interfaces
{
    public interface IBusRepository
    {
        public Task<List<Bus>> GetAllAsync(BusParametrosConsulta parametrosConsulta);
        public Task<int> GetTotalAsync();
        public Task<Bus> GetByIdAsync(Guid busId);
        public Task<Bus> GetByDetailsAsync(Bus unBus);
        public Task<bool> CreateAsync(Bus unBus);
        public Task<bool> UpdateAsync(Bus unBus);
        public Task<bool> RemoveAsync(Guid busId);
    }
}
