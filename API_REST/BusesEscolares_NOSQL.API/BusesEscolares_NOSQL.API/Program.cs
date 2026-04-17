using Asp.Versioning;
using BusesEscolares_NOSQL.API.DbContexts;
using BusesEscolares_NOSQL.API.Interfaces;
using BusesEscolares_NOSQL.API.Repositories;
using BusesEscolares_NOSQL.API.Services;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

//Aqui agregamos los servicios requeridos

// ***************************************************************************
// --- Configuraci�n del DB Context --
// ***************************************************************************

builder.Services.AddSingleton<MongoDbContext>();

// ***************************************************************************
// --- Configuraci�n de los repositorios --
// ***************************************************************************

builder.Services.AddScoped<IEstadisticaRepository, EstadisticaRepository>();
builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<IViajeRepository, ViajeRepository>();

// ***************************************************************************
// --- Configuraci�n de los servicios asociados  --
// ***************************************************************************

builder.Services.AddScoped<EstadisticaService>();
builder.Services.AddScoped<BusService>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BusesEscolares_NOSQL.API v1 - MongoDB",
        Description = "API para la gestión de información de Viajes de Buses Escolares"
    });
});

// ***************************************************************************
// --- Configuraci�n del versionamiento para el API  --
// ***************************************************************************

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("api-version"),
        new QueryStringApiVersionReader("api-version")
    );
}
)
    .AddMvc()
    .AddApiExplorer(setup =>
    {
        setup.GroupNameFormat = "'v'VVV";
        setup.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        // Configuración manual de cada endpoint
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "BusesEscolares_NOSQL.API v1");
    }
    );
}

//Modificamos el encabezado de las peticiones para ocultar el web server utilizado
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Server", "ViajesServer");
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();