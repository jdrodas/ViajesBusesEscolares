using BusesEscolares_NOSQL.API.DbContexts;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;
using MongoDB.Driver;

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
    }
}
