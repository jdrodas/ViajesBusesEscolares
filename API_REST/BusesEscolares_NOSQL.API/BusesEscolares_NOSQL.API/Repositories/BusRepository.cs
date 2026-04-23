using BusesEscolares_NOSQL.API.DbContexts;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;
using MongoDB.Driver;

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

        public async Task<Bus> GetByLicensePlateAsync(string busPlaca)
        {
            Bus unBus = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionBuses = conexion
                .GetCollection<Bus>(contextoDB.ConfiguracionColecciones.ColeccionBuses);

            var resultado = await coleccionBuses
                .Find(bus => bus.Placa == busPlaca)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unBus = resultado;

            return unBus;
        }

        public async Task<Bus> GetByDetailsAsync(Bus unBus)
        {
            Bus busExistente = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionBuses = conexion
                .GetCollection<Bus>(contextoDB.ConfiguracionColecciones.ColeccionBuses);

            var builder = Builders<Bus>.Filter;
            var filtro = builder.And(
                builder.Regex(bus => bus.Placa, $"/^{unBus.Placa}$/i"),
                builder.Eq(bus => bus.AñoFabricacion, unBus.AñoFabricacion)
                );

            var resultado = await coleccionBuses
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                busExistente = resultado;

            return busExistente;
        }

        public async Task<long> GetTotalAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionBuses = conexion
                .GetCollection<Bus>(contextoDB.ConfiguracionColecciones.ColeccionBuses);

            var totalBuses = await coleccionBuses
                .EstimatedDocumentCountAsync();

            return totalBuses;
        }

        public async Task<bool> CreateAsync(Bus unBus)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionBuses = conexion
                .GetCollection<Bus>(contextoDB.ConfiguracionColecciones.ColeccionBuses);

            await coleccionBuses
                .InsertOneAsync(unBus);

            var resultado = await GetByDetailsAsync(unBus);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Bus unBus)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionBuses = conexion
                .GetCollection<Bus>(contextoDB.ConfiguracionColecciones.ColeccionBuses);

            var resultado = await coleccionBuses
                .ReplaceOneAsync(bus => bus.Id == unBus.Id, unBus);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> RemoveAsync(string busId)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionBuses = conexion
                .GetCollection<Bus>(contextoDB.ConfiguracionColecciones.ColeccionBuses);

            var resultado = await coleccionBuses
                .DeleteOneAsync(bus => bus.Id == busId);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
