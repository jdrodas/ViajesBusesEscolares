-- Scripts de clase - Abril 19 de 2026
-- Curso de Tópicos Avanzados de base de datos - UPB 202610
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Viajes Buses Escolares
-- Motor de Base de datos: MongoDB Community Edition - 8.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen
docker pull mongodb/mongodb-community-server:latest

-- Crear el contenedor
docker run --name mongodb-vbe -e “MONGO_INITDB_ROOT_USERNAME=mongoadmin” -e MONGO_INITDB_ROOT_PASSWORD=unaClav3 -p 27017:27017 -d mongodb/mongodb-community-server:latest

-- =========================================================
-- Creación base de datos, esquema inicial, login y usuario
-- =========================================================

-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Para conectarse al contenedor con el usuario admin 
mongodb://mongoadmin:unaClav3@localhost:27017/

-- Con usuario mongoadmin:

-- crear la base de datos
use vbe_db;

-- Crear el rol para el usuario de gestion de Documentos en las colecciones
db.createRole(
  {
    role: "GestorDocumentos",
    privileges: [
        {
            resource: { 
                db: "vbe_db", 
                collection: "" 
            }, 
            actions: [
                "find", 
                "insert", 
                "update", 
                "remove",
                "listCollections"
            ]
        }
    ],
    roles: []
  }
);

-- Crear usuario para gestionar el modelo

db.createUser({
  user: "viajes_app",
  pwd: "unaClav3",  
  roles: [
    { role: "readWrite", db: "vbe_db" },
    { role: "dbAdmin", db: "vbe_db" }
  ],
    mechanisms: ["SCRAM-SHA-256"]
  }
);

db.createUser(
  {
    user: "viajes_usr",
    pwd: "unaClav3",
    roles: [ 
    { role: "GestorDocumentos", db: "vbe_db" }
    ],
    mechanisms: ["SCRAM-SHA-256"]
  }
);

-- Para saber que usuarios hay creados en la base de datos
db.getUsers()


