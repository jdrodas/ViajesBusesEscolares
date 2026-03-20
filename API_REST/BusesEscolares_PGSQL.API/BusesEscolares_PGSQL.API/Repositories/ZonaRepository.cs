using BusesEscolares_PGSQL.API.DbContexts;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using Dapper;
using System.Data;

namespace BusesEscolares_PGSQL.API.Repositories
{
    public class ZonaRepository(PgsqlDbContext unContexto) : IZonaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Zona>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre " +
                "FROM core.zonas " +
                "ORDER BY nombre";

            var resultadoZonas = await conexion
                .QueryAsync<Zona>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoZonas];
        }

        public async Task<Zona> GetByIdAsync(Guid zonaId)
        {
            Zona unaZona = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@zonaId", zonaId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre " +
                "FROM core.zonas " +
                "WHERE id = @zonaId";

            var resultado = await conexion
                .QueryAsync<Zona>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaZona = resultado.First();

            return unaZona;
        }
    }
}
