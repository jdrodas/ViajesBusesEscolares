using BusesEscolares_NOSQL.API.DbContexts;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;
using MongoDB.Driver;
using static System.Net.WebRequestMethods;

namespace BusesEscolares_NOSQL.API.Repositories
{
    public class ViajeRepository(MongoDbContext unContexto) : IViajeRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<List<Viaje>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var losViajes = await coleccionViajes
                .Find(_ => true)
                .SortBy(viaje => viaje.FechaSalida)
                .ToListAsync();

            return losViajes;
        }

        public async Task<Viaje> GetByDetailsAsync(Viaje unViaje)
        {
            Viaje viajeExistente = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var builder = Builders<Viaje>.Filter;
            var filtro = builder.And(
                builder.Eq(viaje => viaje.BusId, unViaje.BusId),
                builder.Eq(viaje => viaje.RutaId, unViaje.RutaId),
                builder.Regex(viaje => viaje.FechaSalida, $"/^{unViaje.FechaSalida}$/i")
                );

            var resultado = await coleccionViajes
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                viajeExistente = resultado;

            return viajeExistente;
        }

        public async Task<List<Viaje>> GetAssociatedTripsToBusByIdAsync(string busId)
        {
            var conexion = contextoDB
                            .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var losViajes = await coleccionViajes
                .Find(viaje => viaje.BusId == busId)
                .SortBy(viaje => viaje.FechaSalida)
                .ToListAsync();

            return losViajes;
        }

        public async Task<List<Viaje>> GetAssociatedTripsToRouteByIdAsync(string rutaId)
        {
            var conexion = contextoDB
                            .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var losViajes = await coleccionViajes
                .Find(viaje => viaje.RutaId == rutaId)
                .SortBy(viaje => viaje.FechaSalida)
                .ToListAsync();

            return losViajes;
        }

        public async Task<List<Viaje>> GetAssociatedTripsToZoneByIdAsync(string zonaId)
        {
            var conexion = contextoDB
                            .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var losViajes = await coleccionViajes
                .Find(viaje => viaje.ZonaId == zonaId)
                .SortBy(viaje => viaje.FechaSalida)
                .ToListAsync();

            return losViajes;
        }

        public async Task<Viaje> GetByIdAsync(string viajeId)
        {
            Viaje unViaje = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var resultado = await coleccionViajes
                .Find(viaje => viaje.Id == viajeId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unViaje = resultado;

            return unViaje;
        }

        public async Task<long> GetTotalAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var totalViajes = await coleccionViajes
                .EstimatedDocumentCountAsync();

            return totalViajes;
        }

        public async Task<bool> UpdateBusDataAsync(Bus unBus)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var filtro = Builders<Viaje>.Filter
                .Eq(viaje => viaje.BusId, unBus.Id);
            
            var sentenciaActualizacion = Builders<Viaje>.Update
                .Set(viaje => viaje.BusPlaca, unBus.Placa);

            var resultado = await coleccionViajes
                .UpdateManyAsync(filtro, sentenciaActualizacion);
            
            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateRouteDataAsync(Ruta unaRuta)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var filtro = Builders<Viaje>.Filter
                .Eq(viaje => viaje.RutaId, unaRuta.Id);

            var definicionesActualizacion = new List<UpdateDefinition<Viaje>>
            {
                Builders<Viaje>.Update.Set(viaje => viaje.RutaNombre, unaRuta.Nombre),
                Builders<Viaje>.Update.Set(viaje => viaje.ZonaId, unaRuta.ZonaId),
                Builders<Viaje>.Update.Set(viaje => viaje.ZonaNombre, unaRuta.ZonaNombre)
            };

            var sentenciaActualizacion = Builders<Viaje>.Update
                .Combine(definicionesActualizacion);

            var resultado = await coleccionViajes
                .UpdateManyAsync(filtro, sentenciaActualizacion);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> CreateAsync(Viaje unViaje)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            await coleccionViajes
                .InsertOneAsync(unViaje);

            var resultado = await GetByDetailsAsync(unViaje);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> RemoveAsync(string viajeId)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var resultado = await coleccionViajes
                .DeleteOneAsync(viaje=> viaje.Id == viajeId);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
