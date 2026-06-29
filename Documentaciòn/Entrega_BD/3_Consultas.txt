-- ==========================================================
-- SCRIPT 3: Resolución de Consultas
-- ==========================================================

-- C1. Listado de colmenas que nunca tuvieron ningún tratamiento aplicado, 
-- mostrando su ID, estado, fortaleza y la zona del apiario al que pertenecen.
select c.ID_Colmena, c.Estado, c.Fortaleza_Abejas, a.Zona
from Colmenas c inner join Tiene t
on c.ID_Colmena = t.ID_Colmena
inner join Apiarios a
on t.ID_Apiario = a.ID_Apiario
left join Aplica ap
on ap.ID_Colmena = c.ID_Colmena
where ap.ID_Colmena is null;

-- C2. Listado del o los apiarios con mayor cantidad de colmenas en estado 'Débil', 
-- mostrando el ID del apiario, la zona y la cantidad de colmenas débiles.
select a.ID_Apiario, a.Zona, count(*) as CantColmenasDebiles
from Apiarios a inner join Tiene t
on a.ID_Apiario = t.ID_Apiario
inner join Colmenas c
on t.ID_Colmena = c.ID_Colmena
where c.Fortaleza_Abejas = 'Débil'
group by a.ID_Apiario, a.Zona
having count(*) >= all (
    select count(*)
    from Tiene t2 inner join Colmenas c2
    on t2.ID_Colmena = c2.ID_Colmena
    where c2.Fortaleza_Abejas = 'Débil'
    group by t2.ID_Apiario
);

-- C3. Listado de cosechas con más barriles generados que el promedio, 
-- mostrando el ID, estado, fecha y cantidad de barriles.
select c.ID_Cosecha, c.Estado, r.Fecha_Cosecha, r.Tipo_Miel, r.Cant_Barriles
from Cosecha c inner join Realiza r
on c.ID_Cosecha = r.ID_Cosecha
where r.Cant_Barriles > (
    select avg(cast(Cant_Barriles as float))
    from Realiza
)
order by r.Cant_Barriles desc;

-- C4. Actualizar el estado a 'Critica' a todas las colmenas que pertenecen 
-- a apiarios cuya cantidad de colmenas registradas supera las 50.
update Colmenas
set Estado = 'Critica'
where ID_Colmena in (
    select t.ID_Colmena
    from Tiene t inner join Apiarios a
    on t.ID_Apiario = a.ID_Apiario
    where a.Cant_Colmenas > 50
);

-- C5. Listado de visitas realizadas en temporada de verano, mostrando el ID 
-- de visita, la fecha, la zona del apiario visitado, el clima registrado y la duración en minutos.
select v.ID_Visita, v.Fecha, a.Zona, r.Clima,
    datediff(minute, r.H_Salida, r.H_Llegada) as DuracionMinutos
from Visitas v inner join Revisa r
on v.ID_Visita = r.ID_Visita
inner join Apiarios a
on r.ID_Apiario = a.ID_Apiario
where v.Temporada = 'Verano'
order by v.Fecha desc;

-- C6. Colmenas con reinas en estado 'Baja' o nivel de producción 'Bajo'
SELECT
    c.ID_Colmena,
    c.Estado AS EstadoColmena,
    c.Fortaleza_Abejas,
    rei.ID_Reina,
    rei.Estado AS EstadoReina,
    rei.Nivel_Prod,
    a.ID_Apiario,
    a.Zona
FROM Colmenas c
INNER JOIN Asigna asg ON c.ID_Colmena = asg.ID_Colmena
INNER JOIN Reinas rei ON asg.ID_Reina = rei.ID_Reina
INNER JOIN Tiene t ON c.ID_Colmena = t.ID_Colmena
INNER JOIN Apiarios a ON t.ID_Apiario = a.ID_Apiario
WHERE rei.Estado = 'Baja' OR rei.Nivel_Prod = 'Bajo'
ORDER BY a.Zona, c.ID_Colmena;

-- C7. Eliminar los tratamientos que no fueron aplicados a ninguna colmena 
-- y cuya fecha de inicio es anterior a enero de 2025.
delete from Tratamientos
where Fecha_Inicio < '2025-01-01'
and ID_Tratamiento not in (
    select ID_Tratamiento
    from Aplica
);

-- C8. Aumentar en 10 la cantidad de bastidores de todas las colmenas que pertenecen 
-- a apiarios que tuvieron al menos una cosecha de tipo 'Monofloral' en los últimos 12 meses.
update Colmenas
set Cant_Bastidores = Cant_Bastidores + 10
where ID_Colmena in (
    select t.ID_Colmena
    from Tiene t inner join Realiza r
    on t.ID_Apiario = r.ID_Apiario
    where r.Tipo_Miel = 'Monofloral'
    and r.Fecha_Cosecha >= dateadd(month, -12, getdate())
);

-- C9. Listado de herramientas que fueron planificadas en más de una visita, 
-- mostrando su ID, nombre, tipo y cuántas veces fueron usadas.
select h.ID_Herramienta, h.Nombre, h.Tipo, count(p.ID_Visita) as VecesUsada
from Herramientas h inner join Planifica p
on h.ID_Herramienta = p.ID_Herramienta
group by h.ID_Herramienta, h.Nombre, h.Tipo
having count(p.ID_Visita) > 1
order by VecesUsada;

-- C10. Listado de los 3 apiarios con mayor producción total de miel en toda su historia,
-- mostrando el ID, zona, cantidad de cosechas y total de kg.
select top 3 a.ID_Apiario, a.Zona,
    count(distinct r.ID_Cosecha) as CantCosechas,
    sum(b.Cantidad_Miel) as TotalMielKg
from Apiarios a inner join Realiza r
on a.ID_Apiario = r.ID_Apiario
inner join Genera g
on r.ID_Cosecha = g.ID_Cosecha
inner join Barriles b
on g.ID_Barril = b.ID_Barril
group by a.ID_Apiario, a.Zona
order by TotalMielKg desc;
