-- Scripts de clase - 5 de Marzo de 2026 
-- Curso de Tópicos Avanzados de base de datos - UPB 202610
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Viajes Buses Escolares
-- Motor de Base de datos: PostgreSQL 18.x

-- ***********************************
-- Creación del modelo de datos
-- ***********************************

-- Con el usuario viajes_app

-- ****************************************
-- Creación de Tablas
-- ****************************************

-- Tabla temporal para recibir los datos del archivo base

create table core.datos_temporales
(
    nombre_conductor         text,
    codigo_bus               text,
    año_fabricacion_bus      int,
    codigo_ruta              text,
    nombre_ruta              text,
    distancia_ruta           float,
    zona_ruta                text,
    turno                    text,
    num_pasajeros            int,
    fecha_viaje              text,
    hora_salida              text,
    hora_llegada             text,
    tiempo_minutos           int,
    velocidad_promedio_kmh   float,
    soc_inicial_porcentaje   float,
    soc_final_porcentaje     float,
    soc_consumido_porcentaje float
);

-- Tabla: Buses
create table core.buses
(
    id                              uuid default gen_random_uuid() constraint buses_pk primary key,
    placa                           text not null,
    año_fabricacion                 int not null
);

-- Llenado de datos desde la tabla de datos temporales
insert into core.buses (placa, año_fabricacion)
select distinct codigo_bus,año_fabricacion_bus
from core.datos_temporales
order by codigo_bus;

-- Devolvemos guid generado a la tabla temporal
alter table core.datos_temporales add bus_id uuid;

update core.datos_temporales dt
set bus_id =
    (select id from core.buses
     where placa = dt.codigo_bus)
where dt.bus_id is null;


-- Tabla: zonas
create table core.zonas
(
    id                              uuid default gen_random_uuid() constraint zonas_pk primary key,
    nombre                          text not null
);

-- Llenado de datos desde la tabla de datos temporales
insert into core.zonas (nombre)
select distinct zona_ruta
from core.datos_temporales
order by zona_ruta;

-- Devolvemos guid generado a la tabla temporal
alter table core.datos_temporales add zona_id uuid;

update core.datos_temporales dt
set zona_id =
    (select id from core.zonas
     where nombre = dt.zona_ruta)
where dt.zona_id is null;


-- Tabla: Rutas
create table core.rutas
(
    id                              uuid default gen_random_uuid() not null constraint rutas_pk primary key,
    nombre                          text not null,
    distancia_kms                   float not null,
    zona_id                         uuid not null constraint rutas_zonas_fk references core.zonas
);

insert into core.rutas (nombre, distancia_kms, zona_id)
select distinct
    nombre_ruta,
    distancia_ruta,
    zona_id
from core.datos_temporales;

-- Devolvemos guid generado a la tabla temporal
alter table core.datos_temporales add ruta_id  uuid;

update core.datos_temporales dt
set ruta_id =
    (select id from core.rutas
     where nombre = dt.nombre_ruta)
where dt.ruta_id is null;

-- Tabla viajes
create table core.viajes
(
    id                              uuid default gen_random_uuid() not null constraint viajes_pk primary key,
    ruta_id                         uuid not null constraint viajes_rutas_fk references core.rutas,
    bus_id                          uuid not null constraint viajes_buses_fk references core.buses,
    turno                           text not null,
    total_pasajeros                 int not null,
    fecha_salida                    timestamp without time zone not null,
    fecha_llegada                   timestamp without time zone not null
);

insert into core.viajes (
    ruta_id, bus_id, turno, total_pasajeros, fecha_salida, fecha_llegada)
select distinct
    ruta_id, bus_id, turno, num_pasajeros,
    to_timestamp((fecha_viaje || ' ' || hora_salida),'YYYY-MM-DD HH24:MI:SS') fecha_salida,
    to_timestamp((fecha_viaje || ' ' || hora_llegada),'YYYY-MM-DD HH24:MI:SS') fecha_llegada
from core.datos_temporales;


-- ****************************************
-- Creación de Vistas
-- ****************************************

-- Vista v_info_viajes
create or replace view v_info_viajes as
(
select
    v.id viaje_id,
    v.turno viaje_turno,
    v.total_pasajeros,
    v.ruta_id,
    r.nombre ruta_nombre,
    r.distancia_kms ruta_distancia_kms,
    r.zona_id,
    z.nombre zona_nombre,
    v.bus_id,
    b.placa bus_placa,
    v.fecha_salida,
    v.fecha_llegada,
    (extract(epoch from (v.fecha_llegada - v.fecha_salida)) / 60) duracion_minutos,
    round((r.distancia_kms/(extract(epoch from (v.fecha_llegada - v.fecha_salida)/ 3600)))::numeric,2) velocidad_kmh
from viajes v
    join rutas r on v.ruta_id = r.id
    join zonas z on r.zona_id = z.id
    join buses b on v.bus_id = b.id
);

-- Vista: v_info_rutas
create view v_info_rutas as (
select
    r.id ruta_id,
    r.nombre ruta_nombre,
    r.distancia_kms,
    r.zona_id,
    z.nombre zona_nombre
from core.rutas r
    join zonas z on r.zona_id = z.id);


-- ****************************************
-- Creación de Indices
-- ****************************************

create index viajes_bus_ix on core.viajes (bus_id);
create index viajes_ruta_ix on core.viajes (ruta_id);
