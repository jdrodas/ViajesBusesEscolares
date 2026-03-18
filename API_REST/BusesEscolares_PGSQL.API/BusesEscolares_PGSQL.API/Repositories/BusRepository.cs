using BusesEscolares_PGSQL.API.DbContexts;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using Dapper;
using System.Data;
using System.Drawing;

namespace BusesEscolares_PGSQL.API.Repositories
{
    public class BusRepository(PgsqlDbContext unContexto) : IBusRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;
        public async Task<List<Bus>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT id, placa, año_fabricacion añoFabricacion " +
                "FROM core.buses ORDER BY placa";

            var resultadoBuses = await conexion
                .QueryAsync<Bus>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoBuses];
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
