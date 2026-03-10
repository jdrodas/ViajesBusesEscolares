-- Scripts de clase - 9 de Marzo de 2026 
-- Curso de Tópicos Avanzados de base de datos - UPB 202610
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Viajes Buses Escolares
-- Motor de Base de datos: PostgreSQL 18.x

-- ***********************************
-- Creación del modelo de datos
-- ***********************************

-- Con el usuario viajes_app

-- ****************************************
-- Creación de procedimientos almacenados
-- ****************************************

-- ### Buses ####
/*
    id                              uuid default gen_random_uuid() constraint buses_pk primary key,
    placa                           text not null,
    año_fabricacion                 int not null
*/

-- p_inserta_bus
create or replace procedure core.p_inserta_bus(
                            in p_placa                  text,
                            in p_año_fabricacion        integer)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_placa is null or
           p_año_fabricacion is null or
           length(p_placa) = 0 or
           p_año_fabricacion <= 2000 then
               raise exception 'La placa del bus o su año de fabricación son nulos o inválidos.';
        end if;

        select count(id) into l_total_registros
        from core.buses
        where upper(p_placa) = upper(placa);

        if l_total_registros != 0  then
            raise exception 'ya existe ese bus registrado con esa placa';
        end if;

        insert into core.buses (placa,año_fabricacion)
        values (upper(p_placa), p_año_fabricacion);
    end;
$$;

-- p_actualiza_bus
create or replace procedure core.p_actualiza_bus(
                            in p_id                     uuid,
                            in p_placa                  text,
                            in p_año_fabricacion        integer)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_placa is null or
           p_año_fabricacion is null or
           p_id is null or
           length(p_placa) = 0 or
           p_año_fabricacion <= 2000 then
               raise exception 'El Id, la placa del bus o su año de fabricación son nulos o inválidos.';
        end if;

        select count(id) into l_total_registros
        from core.buses
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un bus con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.buses
        where upper(p_placa) = upper(placa)
        and año_fabricacion = p_año_fabricacion
        and id != p_id;

        if l_total_registros > 0  then
            raise exception 'Ya existe un bus registrado con esa placa y año de fabricación';
        end if;

        update core.buses
        set
            placa = upper(p_placa),
            año_fabricacion = p_año_fabricacion
        where id = p_id;
    end;
$$;

-- p_elimina_bus
create or replace procedure core.p_elimina_bus(
                            in p_id                     uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        select count(id) into l_total_registros
        from core.buses
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un bus con ese Id';
        end if;

        select count(bus_id) into l_total_registros
        from core.v_info_viajes
        where bus_id = p_id;

        if l_total_registros != 0  then
            raise exception 'No se puede eliminar, hay viajes registrados que dependen de este bus.';
        end if;

        delete from core.buses
        where id = p_id;
    end;
$$;

-- ### Rutas ####
/*
    id                              uuid default gen_random_uuid() not null constraint rutas_pk primary key,
    nombre                          text not null,
    distancia_kms                   float not null,
    zona_id                         uuid not null constraint rutas_zonas_fk references core.zonas
*/

-- p_inserta_ruta
create or replace procedure core.p_inserta_ruta(
                            in p_nombre                 text,
                            in p_distancia_kms          float,
                            in p_zona_id                uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_nombre is null or
           p_distancia_kms is null or
           p_zona_id is null or
           length(p_nombre) = 0 or
           p_distancia_kms <=0 then
            raise exception 'El nombre, distancia o zona de la ruta son nulos o inválidos';
        end if;

        select count(id) into l_total_registros
        from core.rutas
        where upper(p_nombre) = upper(nombre);

        if l_total_registros != 0  then
            raise exception 'ya existe esa ruta registrada con ese nombre';
        end if;

        select count(id) into l_total_registros
        from core.zonas
        where id = p_zona_id;

        if l_total_registros = 0  then
            raise exception 'No existe una zona registrada con ese id';
        end if;

        insert into core.rutas (nombre,distancia_kms,zona_id)
        values (initcap(p_nombre),p_distancia_kms, p_zona_id);
    end;
$$;

-- p_actualiza_ruta

-- p_elimina_ruta
create or replace procedure core.p_elimina_ruta(
                            in p_id                     uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        select count(id) into l_total_registros
        from core.rutas
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe una con ese Id';
        end if;

        select count(ruta_id) into l_total_registros
        from core.v_info_viajes
        where ruta_id = p_id;

        if l_total_registros != 0  then
            raise exception 'No se puede eliminar, hay viajes registrados que dependen de esta ruta.';
        end if;

        delete from core.rutas
        where id = p_id;
    end;
$$;



-- Ejecuciones de prueba de los procedimientos:

do
$$
    begin
        call core.p_inserta_bus(p_placa := null, p_año_fabricacion := 0);
    end
$$;

do
$$
    begin
        call core.p_actualiza_bus(p_id := null, p_placa := null, p_año_fabricacion := 0);
    end
$$;

do
$$
    begin
        call core.p_elimina_bus(p_id := null);
    end
$$;


do
$$
    begin
        call core.p_inserta_ruta(p_nombre := null, p_distancia_kms := null, p_zona_id := null);
    end
$$;