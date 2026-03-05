# Viajes de buses escolares

Repositorio del proyecto de gestión de información de Viajes escolares.

APIs REST desarrolladas como ejercicio demostrativo para el curso de **Tópicos Avanzados de Bases de Datos**, enfocadas en la implementación del **patrón repositorio** y la separación por capas. El dominio de problema aborda el registro de la información de viajes, buses y rutas.

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/jdrodas/ViajesBusesEscolares)
![License](https://img.shields.io/badge/license-Academic-orange)
![.NET](https://img.shields.io/badge/.NET-10.0-purple)
![PostgreSQL](https://img.shields.io/badge/database-postgreSQL-blue)
![MongoDB](https://img.shields.io/badge/database-mongoDB-green)

## Objetivos Académicos

- Demostrar la implementación del **patrón repositorio.**
- Evidenciar la separación clara entre capas de la aplicación.
- Mostrar el desacople de la capa de persistencia para intercambio de bases de datos.
- Implementar versionamiento de APIs siguiendo estándares de la industria.
- Implementar paginación en las respuestas de las peticiones con gran cantidad de resultados.
- Aplicar mejores prácticas de seguridad usando GUIDs en lugar de IDs secuenciales.

## Modelo de Datos

### Entidades Principales

#### Viajes

- `Id` (UUID): Identificador único del viaje.
- `ruta_id` (UUID): Identificador único de la ruta.
- `bus_id` (UUID): Identificador único del bus.
- `turno` (TEXT): Identifica la jornada del día para el viaje.
- `total_pasajeros` (INT): Cantidad de pasajeros transportados en el viaje.
- `fecha_salida` (DATETIME): Fecha y hora del inicio del viaje.
- `fecha_llegada` (DATETIME): Fecha y hora del finalización del viaje.

#### Buses

- `Id` (UUID): Identificador único del bus.
- `placa` (TEXT): Placa asignada al bus.
- `año_fabricacion` (INT): Año para el cual el vehículo fue ensamblado.

#### Rutas

- `Id` (UUID): Identificador único de la ruta.
- `nombre` (TEXT): Nombre de la ruta.
- `distancia_kms` (FLOAT): Distancia en Kms en la ruta.
- `zona_id` (UUID): Identificador único de la zona.


### Origen de los datos

Los datos utilizados para este proyecto son **datos sintéticos** que no corresponden a ninguna representación real de ningún recorrido, por lo tanto no se está mal manejo de datos reservados.


## Stack Tecnológico

- **Framework base**: C# en .NET 10.x
- **Base de Datos**: PostgreSQL 18.x / MongoDB 8.x
- **ORM**: Dapper (micro-ORM) 2.1.66
- **Documentación**: Swagger/OpenAPI usando Swashbuckle 10.0.1
- **Driver DB Relacional**: Npgsql 10.0.0

## Arquitectura - API REST

### Estructura de Capas

```
Controllers → Services → Repositories (via Interfaces) → DB Context
                  ↓
              IRepositories (Interfaces)
```

### Componentes

- **Controllers**: Capa de presentación y manejo de HTTP.
- **Services**: Lógica de negocio y reglas de dominio.
- **Interfaces**: Contratos para desacoplamiento.
- **Repositories**: Implementaciones de acceso a datos.
- **Models**: Modelos de dominio.
- **DBContext**: Contextos y configuraciones de base de datos.

## Endpoints - API REST

El versionamiento de los endpoints utilizará parámetro en el encabezado o parámetro de consulta, en lugar de incluirlo en el URL

### Viajes

```http
GET    /api/viajes                       # Listar todos los viajes
GET    /api/viajes/{id}                  # Obtener un viaje por ID
GET    /api/viajes/{fechaId}             # Obtener viajes realizados por fecha
POST   /api/viajes                       # Crear nuevo viajes
PUT    /api/viajes                       # Actualizar viajes
DELETE /api/viajes/{id}                  # Eliminar viajes
```

### Buses               

```http
GET    /api/buses                         # Listar todos los buses
GET    /api/buses/{id}                    # Obtener un bus por ID
GET    /api/buses/{fechaId}               # Obtener buses que trabajaron por fecha
POST   /api/buses                         # Crear nuevo color
PUT    /api/buses                         # Actualizar color
DELETE /api/buses/{id}                    # Eliminar color
```

### Rutas

```http
GET    /api/rutas                        # Listar todas las rutas
GET    /api/rutas/{id}                   # Obtener una ruta por ID
GET    /api/rutas/{fechaId}              # Obtener rutas recorridas por fecha
POST   /api/rutas                        # Crear nueva ruta
PUT    /api/rutas/                       # Actualizar ruta
DELETE /api/rutas/{id}                   # Eliminar ruta
```
