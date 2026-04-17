using BusesEscolares_NOSQL.API.DbContexts;
using BusesEscolares_NOSQL.API.Models;


namespace BusesEscolares_NOSQL.API.Repositories
{
    public class EstadisticaRepository(MongoDbContext unContexto) : IEstadisticaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<Estadistica> GetAllAsync()
        {
            Estadistica unaEstadistica = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionBuses = conexion
                .GetCollection<Bus>(contextoDB.ConfiguracionColecciones.ColeccionBuses);

            var totalBuses = await coleccionBuses
                .EstimatedDocumentCountAsync();

            unaEstadistica.Buses = totalBuses;

            var coleccionRutas = conexion
                .GetCollection<Ruta>(contextoDB.ConfiguracionColecciones.ColeccionRutas);

            var totalRutas = await coleccionRutas
                .EstimatedDocumentCountAsync();

            unaEstadistica.Rutas = totalRutas;

            var coleccionViajes = conexion
                .GetCollection<Viaje>(contextoDB.ConfiguracionColecciones.ColeccionViajes);

            var totalViajes = await coleccionViajes
                .EstimatedDocumentCountAsync();

            unaEstadistica.Viajes = totalViajes;

            return unaEstadistica;
        }
    }
}