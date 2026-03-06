-- Scripts de clase - 5 de Marzo de 2026 
-- Curso de Tópicos Avanzados de base de datos - UPB 202610
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Viajes Buses Escolares
-- Motor de Base de datos: PostgreSQL 18.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen
docker pull postgres:latest

-- Crear el contenedor
docker run --name pgsql-vbe -e POSTGRES_PASSWORD=unaClav3 -d -p 5432:5432 postgres:latest


-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Con usuario Postgres:

-- crear el esquema la base de datos
create database vbe_db;

-- Conectarse a la base de datos
\c vbe_db;

-- Creamos un esquema para almacenar todo el modelo de datos del dominio
create schema core;

-- crear el usuario con el que se implementará la creación del modelo
create user viajes_app with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database vbe_db to viajes_app;
grant create on database vbe_db to viajes_app;
grant create, usage on schema core to viajes_app;
alter user viajes_app set search_path to core;

-- crear el usuario con el que se conectará la aplicación
create user viajes_usr with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database vbe_db to viajes_usr;
grant usage on schema core to viajes_usr;

-- Privilegios sobre tablas existentes
grant select, insert, update, delete, trigger on all tables in schema core to viajes_usr;

-- privilegios sobre secuencias existentes
grant usage, select on all sequences in schema core to viajes_usr;

-- privilegios sobre funciones existentes
grant execute on all functions in schema core to viajes_usr;

-- privilegios sobre procedimientos existentes
grant execute on all procedures in schema core to viajes_usr;

-- privilegios sobre objetos futuros
alter default privileges in schema core grant select, insert, update, delete on tables TO viajes_usr;
alter default privileges in schema core grant execute on routines to viajes_usr;

alter user viajes_usr set search_path to core;

-- Activar la extensión que permite el uso de UUID
create extension if not exists "uuid-ossp";