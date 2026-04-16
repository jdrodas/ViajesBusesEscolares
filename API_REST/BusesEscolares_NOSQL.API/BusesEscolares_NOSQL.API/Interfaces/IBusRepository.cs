using BusesEscolares_NOSQL.API.Models;

namespace BusesEscolares_NOSQL.API.Interfaces
{
    public interface IBusRepository
    {
        public Task<List<Bus>> GetAllAsync();
        //public Task<int> GetTotalAsync();
        public Task<Bus> GetByIdAsync(string busId);
        //public Task<Bus> GetByDetailsAsync(Bus unBus);
        //public Task<bool> CreateAsync(Bus unBus);
        //public Task<bool> UpdateAsync(Bus unBus);
        //public Task<bool> RemoveAsync(string busId);
    }
}
