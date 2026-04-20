using BusesEscolares_NOSQL.API.DbContexts;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;
using MongoDB.Driver;

namespace BusesEscolares_NOSQL.API.Repositories
{
    public class RutaRepository(MongoDbContext unContexto) : IRutaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<List<Ruta>> GetAllAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionRutas = conexion
                .GetCollection<Ruta>(contextoDB.ConfiguracionColecciones.ColeccionRutas);

            var lasRutas = await coleccionRutas
                .Find(_ => true)
                .SortBy(ruta => ruta.Nombre)
                .ToListAsync();

            return lasRutas;
        }

        public async Task<long> GetTotalAsync()
        {
            var conexion = contextoDB
                .CreateConnection();

            var coleccionRutas = conexion
                .GetCollection<Ruta>(contextoDB.ConfiguracionColecciones.ColeccionRutas);

            var totalRutas = await coleccionRutas
                .EstimatedDocumentCountAsync();

            return totalRutas;
        }

        public async Task<Ruta> GetByIdAsync(string rutaId)
        {
            Ruta unaRuta = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionRutas = conexion
                .GetCollection<Ruta>(contextoDB.ConfiguracionColecciones.ColeccionRutas);

            var resultado = await coleccionRutas
                .Find(ruta => ruta.Id == rutaId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaRuta = resultado;

            return unaRuta;
        }

        public async Task<Ruta> GetByDetailsAsync(Ruta unaRuta)
        {
            Ruta rutaExistente = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionRutas = conexion
                .GetCollection<Ruta>(contextoDB.ConfiguracionColecciones.ColeccionRutas);

            var builder = Builders<Ruta>.Filter;
            var filtro = builder.And(
                builder.Regex(ruta => ruta.Nombre, $"/^{unaRuta.Nombre}$/i"),
                builder.Eq(ruta => ruta.DistanciaKms, unaRuta.DistanciaKms),
                builder.Regex(ruta => ruta.ZonaNombre, $"/^{unaRuta.ZonaNombre}$/i")
                );

            var resultado = await coleccionRutas
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                rutaExistente = resultado;

            return rutaExistente;
        }

        public async Task<bool> CreateAsync(Ruta UnaRuta)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionRutas = conexion
                .GetCollection<Ruta>(contextoDB.ConfiguracionColecciones.ColeccionRutas);

            await coleccionRutas
                .InsertOneAsync(UnaRuta);

            var resultado = await GetByDetailsAsync(UnaRuta);

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Ruta UnaRuta)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionRutas = conexion
                .GetCollection<Ruta>(contextoDB.ConfiguracionColecciones.ColeccionRutas);

            var resultado = await coleccionRutas
                .ReplaceOneAsync(ruta => ruta.Id == UnaRuta.Id, UnaRuta);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> RemoveAsync(string rutaId)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB
                .CreateConnection();

            var coleccionRutas = conexion
                .GetCollection<Ruta>(contextoDB.ConfiguracionColecciones.ColeccionRutas);

            var resultado = await coleccionRutas
                .DeleteOneAsync(ruta => ruta.Id == rutaId);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
