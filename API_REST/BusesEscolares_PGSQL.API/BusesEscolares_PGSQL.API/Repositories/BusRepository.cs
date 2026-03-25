using BusesEscolares_PGSQL.API.DbContexts;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using Dapper;
using System.Data;

namespace BusesEscolares_PGSQL.API.Repositories
{
    public class BusRepository(PgsqlDbContext unContexto) : IBusRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;
        public async Task<List<Bus>> GetAllAsync(BusParametrosConsulta parametrosConsulta)
        {
            var conexion = contextoDB.CreateConnection();

            //parametros de paginacion
            var desfase = (parametrosConsulta.Pagina - 1) * parametrosConsulta.ElementosPorPagina;

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@elementosPagina", parametrosConsulta.ElementosPorPagina,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@desfase", desfase,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, placa, año_fabricacion añoFabricacion " +
                "FROM core.buses " +
                "ORDER BY placa " +
                "LIMIT @elementosPagina " +
                "OFFSET @desfase";

            var resultadoBuses = await conexion
                .QueryAsync<Bus>(sentenciaSQL, parametrosSentencia);

            return [.. resultadoBuses];
        }

        public async Task<int> GetTotalAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT COUNT(id) total FROM core.buses";

            var totalBuses = await conexion
                .QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

            return totalBuses;
        }

        public async Task<Bus> GetByIdAsync(Guid busId)
        {
            Bus unBus = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@busId", busId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, placa, año_fabricacion añoFabricacion " +
                "FROM core.buses " +
                "WHERE id = @busId";

            var resultado = await conexion
                .QueryAsync<Bus>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unBus = resultado.First();

            return unBus;
        }
    }
}
