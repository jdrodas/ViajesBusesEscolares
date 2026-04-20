-- Scripts de clase - Abril 19 de 2026
-- Curso de Tópicos Avanzados de base de datos - UPB 202610
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Viajes Buses Escolares
-- Motor de Base de datos: MongoDB Community Edition - 8.x

-- ***********************************
-- Creación del modelo de datos
-- ***********************************

-- Con el usuario viajes_app

-- String de conexión:
mongodb://viajes_app:unaClav3@localhost:27017/?authMechanism=SCRAM-SHA-256&authSource=vbe_db

-- Para autenticación en mongosh

  db.auth("viajes_app", passwordPrompt())


use vbe_db;

-- *******************************************************
-- Creación de Colecciones -- Sin JSON Schema Validator
-- *******************************************************

db.createCollection("zonas");
db.createCollection("buses");
db.createCollection("rutas");
db.createCollection("viajes");

-- *******************************************************
-- Sentencias para actualizar referencias 
-- al objectId generado por mongoDB
-- *******************************************************

-- Actualizar referencia de la zona en las rutas
db.rutas.find().forEach(function(ruta){
  let zona = db.zonas.findOne({"codigo_zona":ruta.zona_codigo});

  if(zona){
    db.rutas.updateOne(
      {_id:ruta._id},
      {$set:{"zona_id": zona._id}}
    )
  }
}
);

-- Renombrar campo nombre de la ruta
db.rutas.updateMany(
   {}, 
   { $rename: { "ruta_nombre": "nombre" } }
);

-- Actualizar en viajes, referencias de:
-- ruta_id, zona_id, bus_id

db.buses.find().forEach(function(bus){  
    db.viajes.updateMany(
      {"bus_codigo": bus.codigo_bus},
      {$set: {"bus_id":bus._id}}
    )
  }
);

db.zonas.find().forEach(function(zona){  
    db.viajes.updateMany(
      {"zona_codigo": zona.codigo_zona},
      {$set: {"zona_id":zona._id}}
    )
  }
);


db.rutas.find().forEach(function(ruta){  
    db.viajes.updateMany(
      {"ruta_codigo": ruta.ruta_codigo},
      {$set: {"ruta_id":ruta._id}}
    )
  }
);

-- Para limpiar colecciones

-- En zonas, quitar campo codigo_zona
db.zonas.updateMany({},{$unset:{codigo_zona:""}});

-- En rutas, quitar campos zona_codigo, ruta_codigo
db.rutas.updateMany({},{$unset:{zona_codigo:""}});
db.rutas.updateMany({},{$unset:{ruta_codigo:""}});

-- En buses, quitar campo codigo_bus
db.buses.updateMany({},{$unset:{codigo_bus:""}});


-- En viajes, quitar bus_codigo, ruta_codigo, zona_codigo
db.viajes.updateMany({},{$unset:{bus_codigo:""}});
db.viajes.updateMany({},{$unset:{ruta_codigo:""}});
db.viajes.updateMany({},{$unset:{zona_codigo:""}});

db.viajes.updateMany({},{$unset:{viaje_codigo:""}});

-- borrado de colecciones temporales
db.buses.drop();
db.viajes.drop();
db.rutas.drop();
db.zonas.drop();

-- *******************************************************
-- Creación de Colecciones -- Con JSON Schema Validator
-- *******************************************************

-- Colección: Zonas
db.createCollection("zonas",{
  validator:{
    $jsonSchema: {
      bsonType: 'object',
      required: [
        '_id',
        'nombre'
      ],
      properties: {
        _id: {
          bsonType: 'objectId'
        },
        nombre: {
          bsonType: 'string'
        }
      },
        additionalProperties: false
    }
  }
});

-- Colección: Rutas
db.createCollection("rutas",{
  validator: {
    $jsonSchema: {
      bsonType: 'object',
      required: [
        '_id',
        'distancia_kms',
        'nombre',
        'zona_nombre',
        'zona_id'
      ],
      properties: {
        _id: {
          bsonType: 'objectId'
        },
        distancia_kms: {
          bsonType: 'double'
        },
        nombre: {
          bsonType: 'string'
        },
        zona_nombre: {
          bsonType: 'string'
        },
        zona_id: {
          bsonType: [
            'objectId',
            'string'
          ] 
        }
      },
      additionalProperties: false
    }
  }
});

-- Colección: buses
db.createCollection("buses",{
  validator: {
      "$jsonSchema": {
        "bsonType": "object",
        "required": [
          "_id",
          "año_fabricacion",
          "placa"
        ],
        "properties": {
          _id: {
            "bsonType": "objectId"
          },
          "año_fabricacion": {
            "bsonType": "int"
          },
          placa: {
            "bsonType": "string"
          }
        },
        additionalProperties: false
      }
    }
  }
);

-- Colección: Viajes:
db.createCollection("viajes",{
  validator:{
    $jsonSchema: {
      bsonType: 'object',
      required: [
        '_id',
        'bus_id',
        'ruta_id',
        'zona_id',
        'bus_placa',
        'ruta_nombre',
        'zona_nombre',      
        'fecha_llegada',
        'fecha_salida',
        'duracion_minutos',
        'total_pasajeros',
        'turno'
      ],
      properties: {
        _id: {
          bsonType: 'objectId'
        },
        bus_id: {
            bsonType: [
              'objectId',
              'string'
            ] 
        },
        ruta_id: {
            bsonType: [
              'objectId',
              'string'
            ] 
        },
        zona_id: {
            bsonType: [
              'objectId',
              'string'
            ] 
        },
        bus_placa: {
            "bsonType": "string"
        },
        ruta_nombre: {
            "bsonType": "string"
        },
        zona_nombre: {
            "bsonType": "string"
        },      
        fecha_salida: {
          bsonType: 'string'
        },
        fecha_llegada: {
          bsonType: 'string'
        },
        duracion_minutos: {
          bsonType: 'int'
        },
        total_pasajeros: {
          bsonType: 'int'
        },
        turno: {
          bsonType: 'string'
        }
      },
      additionalProperties: false
    }
  }
});

