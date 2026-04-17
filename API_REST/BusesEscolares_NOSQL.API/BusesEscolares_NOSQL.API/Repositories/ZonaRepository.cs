

using BusesEscolares_NOSQL.API.DbContexts;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Models;
using MongoDB.Driver;

namespace BusesEscolares_NOSQL.API.Repositories
{
    public class ZonaRepository(MongoDbContext unContexto) : IZonaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<Zona> GetByNameAsync(string zonaNombre)
        {
            Zona unaZona = new();

            var conexion = contextoDB
                .CreateConnection();

            var coleccionZonas = conexion
                .GetCollection<Zona>(contextoDB.ConfiguracionColecciones.ColeccionZonas);

            var resultado = await coleccionZonas
                .Find(zona => zona.Nombre == zonaNombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaZona = resultado;

            return unaZona;
        }
    }
}
