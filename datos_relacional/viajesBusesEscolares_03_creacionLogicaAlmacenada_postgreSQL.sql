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
        if p_id is null then
               raise exception 'El Id no puede ser nulo.';
        end if;

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
create or replace procedure core.p_actualiza_ruta(
                            in p_id                     uuid,
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
           p_id is null or
           length(p_nombre) = 0 or
           p_distancia_kms <=0 then
            raise exception 'El nombre, distancia o zona de la ruta son nulos o inválidos';
        end if;

        select count(id) into l_total_registros
        from core.rutas
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe ruta registrada con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.rutas
        where upper(nombre) = upper(p_nombre)
        and distancia_kms = p_distancia_kms
        and zona_id = p_zona_id
        and id != p_id;

        if l_total_registros != 0  then
            raise exception 'Ya existe una ruta registrada con esos datos pero con diferente Id';
        end if;        

        select count(id) into l_total_registros
        from core.zonas
        where id = p_zona_id;

        if l_total_registros = 0  then
            raise exception 'No existe una zona registrada con ese id';
        end if;

        -- Aqui viene la actualización
        update core.rutas
        set 
            nombre = initcap(p_nombre),
            distancia_kms = p_distancia_kms,
            zona_id = p_zona_id
        where id = p_id;
    end;
$$;


-- p_elimina_ruta
create or replace procedure core.p_elimina_ruta(
                            in p_id                     uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_id is null then
               raise exception 'El Id no puede ser nulo.';
        end if;

        select count(id) into l_total_registros
        from core.rutas
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe una ruta con ese Id';
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

-- ### Viajes ####

/*
    id                              uuid default gen_random_uuid() not null constraint viajes_pk primary key,
    ruta_id                         uuid not null constraint viajes_rutas_fk references core.rutas,
    bus_id                          uuid not null constraint viajes_buses_fk references core.buses,
    turno                           text not null,
    total_pasajeros                 int not null,
    fecha_salida                    timestamp without time zone not null,
    fecha_llegada                   timestamp without time zone not null
*/

-- p_inserta_viaje
create or replace procedure core.p_inserta_viaje(
                            in p_ruta_id                uuid,
                            in p_bus_id                 uuid,
                            in p_turno                  text,
                            in p_total_pasajeros        integer,
                            in p_fecha_salida           text,
                            in p_fecha_llegada          text)    
language plpgsql as
$$
    declare
        l_total_registros   integer;
        l_fecha_llegada     timestamp without time zone;        
        l_fecha_salida      timestamp without time zone;

    begin
        if p_ruta_id is null or
           p_bus_id is null or
           p_turno is null or
           length(p_turno) = 0 or
           p_total_pasajeros is null or
           p_total_pasajeros <=0 or
           p_fecha_salida is null or
           p_fecha_llegada is null or
           length(p_fecha_salida) = 0 or
           length(p_fecha_llegada) = 0 or           
            then
            raise exception 'Los datos del viaje son nulos o inválidos. Revise pues!';
        end if; 

        -- Validar que la ruta exista
        select count(r.id) into l_total_registros
        from core.rutas r
        where r.id = p_ruta_id;

        if l_total_registros = 0  then
            raise exception 'No existe una ruta registrada con ese id';
        end if;        

        select count(b.id) into l_total_registros
        from core.buses b
        where b.id = p_bus_id;

        if l_total_registros = 0  then
            raise exception 'No existe un bus registrado con ese id';
        end if;     

        if upper(p_turno) != 'AM' and p_turno != 'PM' then
            raise exception 'El valor del turno debe ser AM o PM';
        end if;     

        l_fecha_salida = to_timestamp(p_fecha_salida,'YYYY-MM-DD HH24:MI:SS');
        l_fecha_llegada = to_timestamp(p_fecha_llegada,'YYYY-MM-DD HH24:MI:SS');

        if l_fecha_llegada < l_fecha_salida then
            raise exception 'Las fechas de salida y llegada no delimitan un lapso de tiempo';
        end if;

        insert into core.viajes (
            ruta_id, 
            bus_id, 
            turno, 
            total_pasajeros, 
            fecha_salida, 
            fecha_llegada)
        values (
            p_ruta_id,
            p_bus_id,
            upper(p_turno),
            p_total_pasajeros,
            l_fecha_salida,
            l_fecha_llegada
        );

    end;
$$;    

-- p_actualiza_viaje
create or replace procedure core.p_actualiza_viaje(
                            in p_id                     uuid,
                            in p_ruta_id                uuid,
                            in p_bus_id                 uuid,
                            in p_turno                  text,
                            in p_total_pasajeros        integer,
                            in p_fecha_salida           text,
                            in p_fecha_llegada          text)    
language plpgsql as
$$
    declare
        l_total_registros   integer;
        l_fecha_llegada     timestamp without time zone;        
        l_fecha_salida      timestamp without time zone;        

    begin
        if p_id is null or 
            p_ruta_id is null or
            p_bus_id is null or
            p_turno is null or
            length(p_turno) = 0 or
            p_total_pasajeros is null or
            p_total_pasajeros <=0 or
           p_fecha_salida is null or
           p_fecha_llegada is null or
           length(p_fecha_salida) = 0 or
           length(p_fecha_llegada) = 0 then
            raise exception 'Los datos del viaje son nulos o inválidos. Revise pues!';
        end if; 

        select count(v.id) into l_total_registros
        from core.viajes v
        where v.id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un viaje registrado con ese id';
        end if;           

        select count(r.id) into l_total_registros
        from core.rutas r
        where r.id = p_ruta_id;

        if l_total_registros = 0  then
            raise exception 'No existe una ruta registrada con ese id';
        end if;        

        select count(b.id) into l_total_registros
        from core.buses b
        where b.id = p_bus_id;

        if l_total_registros = 0  then
            raise exception 'No existe un bus registrado con ese id';
        end if;     

        if upper(p_turno) != 'AM' and p_turno != 'PM' then
            raise exception 'El valor del turno debe ser AM o PM';
        end if;     

        l_fecha_salida = to_timestamp(p_fecha_salida,'YYYY-MM-DD HH24:MI:SS');
        l_fecha_llegada = to_timestamp(p_fecha_llegada,'YYYY-MM-DD HH24:MI:SS');

        if l_fecha_llegada < l_fecha_salida then
            raise exception 'Las fechas de salida y llegada no delimitan un lapso de tiempo';
        end if;

        select count(v.id) into l_total_registros
        from core.viajes v
        where ruta_id = p_ruta_id
        and bus_id = p_bus_id
        and upper(p_turno) = p_turno
        and total_pasajeros = p_total_pasajeros
        and fecha_salida = l_fecha_salida
        and fecha_llegada = l_fecha_llegada
        and id != p_id;

        if l_total_registros != 0  then
            raise exception 'Ya existe un viaje registrado con esos detalles';
        end if;

        update viajes 
        set 
            ruta_id = p_ruta_id,
            bus_id = p_bus_id,
            turno = upper(p_turno),
            total_pasajeros = p_total_pasajeros,
            fecha_salida = l_fecha_salida,
            fecha_llegada = l_fecha_llegada
        where id = p_id;

    end;
$$;

-- p_elimina_viaje
create or replace procedure core.p_elimina_viaje(
                            in p_id                     uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_id is null then
               raise exception 'El Id no puede ser nulo.';
        end if;

        select count(id) into l_total_registros
        from core.viajes
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un viaje con ese Id';
        end if;

        delete from core.viaje
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

do
$$
    begin
        call core.p_actualiza_ruta(
                p_id := null,
                p_nombre := null,
                p_distancia_kms := null,
                p_zona_id := null
             );
    end
$$;


do
$$
    begin
        call core.p_elimina_ruta(p_id := null);
    end
$$;

do
$$
    begin
        call core.p_inserta_viaje(
                p_ruta_id := '1256dbd3-335b-440b-bd82-2d65f699eb05',
                p_bus_id := 'd77e531b-0037-413b-89b7-b12af5506d92',
                p_turno := 'AM',
                p_total_pasajeros := 18,
                p_fecha_salida := '2026-03-11 7:16:00',
                p_fecha_llegada := '2026-03-11 8:06:00'             );
    end
$$;

do
$$
    begin
        call core.p_actualiza_viaje(
                p_id := '20b51540-7f3e-49fd-9ec3-00b8a6c93da5',
                p_ruta_id := '1256dbd3-335b-440b-bd82-2d65f699eb05',
                p_bus_id := 'd77e531b-0037-413b-89b7-b12af5506d92',
                p_turno := 'AM',
                p_total_pasajeros := 18,
                p_fecha_salida := '2026-03-11 7:16:00',
                p_fecha_llegada := '2026-03-11 7:36:00'             );
    end
$$;