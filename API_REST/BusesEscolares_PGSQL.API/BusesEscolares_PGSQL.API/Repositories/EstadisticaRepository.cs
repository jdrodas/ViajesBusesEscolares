using BusesEscolares_PGSQL.API.DbContexts;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using Dapper;

namespace BusesEscolares_PGSQL.API.Repositories
{
    public class EstadisticaRepository(PgsqlDbContext unContexto) : IEstadisticaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<Estadistica> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            Estadistica conteoRegistros = new();

            string sentenciaSQL =
                "SELECT COUNT(id) total FROM core.zonas";

            conteoRegistros.Zonas = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            sentenciaSQL =
                 "SELECT COUNT(id) total FROM core.buses";

            conteoRegistros.Buses = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            sentenciaSQL =
                 "SELECT COUNT(id) total FROM core.rutas";

            conteoRegistros.Rutas = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());

            sentenciaSQL =
                 "SELECT COUNT(id) total FROM core.viajes";

            conteoRegistros.Viajes = await conexion
                .QueryFirstAsync<long>(sentenciaSQL, new DynamicParameters());


            return conteoRegistros;
        }
    }
}