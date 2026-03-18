using BusesEscolares_PGSQL.API.DbContexts;
using BusesEscolares_PGSQL.API.Interfaces;
using BusesEscolares_PGSQL.API.Models;
using Dapper;
using System.Data;

namespace BusesEscolares_PGSQL.API.Repositories
{
    public class RutaRepository(PgsqlDbContext unContexto) : IRutaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Ruta>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT r.id, r.nombre, r.distancia_kms distanciaKms, " +
                "z.nombre zonaNombre " +
                "FROM core.rutas r JOIN core.zonas z on r.zona_id = z.id " +
                "ORDER BY nombre";

            var resultadoRutas = await conexion
                .QueryAsync<Ruta>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoRutas];
        }

        public async Task<Ruta> GetByIdAsync(Guid rutaId)
        {
            Ruta unaRuta = new();
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@rutaId", rutaId,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT r.id, r.nombre, r.distancia_kms distanciaKms, " +
                "z.nombre zonaNombre " +
                "FROM core.rutas r JOIN core.zonas z on r.zona_id = z.id " +
                "WHERE r.id = @rutaId";

            var resultado = await conexion
                .QueryAsync<Ruta>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaRuta = resultado.First();

            return unaRuta;
        }
    }
}
