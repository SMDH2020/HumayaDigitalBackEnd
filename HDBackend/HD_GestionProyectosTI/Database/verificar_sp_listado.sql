-- Verifica si sp_TI_Solicitudes_Listado ya tiene la columna SumaDiasPorAvance
-- (script 33). Si esta consulta no regresa ninguna fila, el script 33 no se
-- aplico correctamente (quedo la version anterior, sin esa columna) y hay
-- que volver a correrlo.
SELECT OBJECT_DEFINITION(OBJECT_ID('dbo.sp_TI_Solicitudes_Listado')) AS DefinicionActual;

-- Alternativa rapida: solo dice SI/NO tiene la columna.
SELECT CASE
           WHEN OBJECT_DEFINITION(OBJECT_ID('dbo.sp_TI_Solicitudes_Listado')) LIKE '%SumaDiasPorAvance%'
           THEN 'OK: el script 33 SI esta instalado'
           ELSE 'FALTA: el script 33 NO esta instalado, hay que volver a ejecutarlo'
       END AS Estado;
