using BusesEscolares_NOSQL.API.DbContexts;
using BusesEscolares_NOSQL.API.Exceptions;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;
using MongoDB.Driver;
using System.Drawing;

namespace BusesEscolares_NOSQL.API.Repositories
{
    public class BusRepository(MongoDbContext unContexto) : IBusRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;
        public async Task<List<Bus>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionBuses = conexion
                .GetCollection<Bus>(contextoDB.ConfiguracionColecciones.ColeccionBuses);

            var losBuses = await coleccionBuses
                .Find(_ => true)
                .SortBy(bus => bus.Placa)
                .ToListAsync();

            return losBuses;
        }



        public async Task<Bus> GetByIdAsync(string busId)
        {
            Bus unBus = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionBuses = conexion
                .GetCollection<Bus>(contextoDB.ConfiguracionColecciones.ColeccionBuses);

            var resultado = await coleccionBuses
                .Find(bus => bus.Id == busId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unBus = resultado;

            return unBus;
        }


    }
}
