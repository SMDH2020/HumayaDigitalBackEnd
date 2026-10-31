-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Pruebas funcionales de Fase 2B
-- Script preparado para REVISION Y EJECUCION MANUAL en el entorno de
-- DESARROLLO. No se ha ejecutado contra ningun servidor.
--
-- Requisitos previos (deben existir YA en la base de desarrollo, este
-- script no los crea ni inventa valores):
--   - Al menos 2 usuarios reales en la tabla que use TI_UsuarioRoles /
--     TI_Solicitudes.SolicitanteUsuarioId. Sustituir @UsrSolicitante,
--     @UsrJefaturaTI, @UsrGerenciaPlaneacion, @UsrAjeno en CADA BLOQUE por
--     UsuarioId reales que YA tengan asignado el rol correspondiente en
--     TI_UsuarioRoles (Activo=1) -- este script NO asigna roles, solo los usa.
--   - Al menos 1 fila activa en dbo.TI_TiposSolicitud.
--   - Exactamente 6 filas activas en dbo.TI_CriteriosPrioridad cuyos Peso
--     sumen 100 (si no es asi, sp_TI_EvaluacionesPrioridad_Crear rechazara
--     el bloque de creacion de evaluacion con el mensaje "El catalogo de
--     criterios activos no tiene 6 elementos..." -- esto NO es un bug de
--     este script, es una validacion real del SP).
--   - Al menos 1 fila activa en dbo.TI_Prioridades (para la prueba de
--     decision PRIORIZAR).
--
-- ESTRUCTURA DE EJECUCION (IMPORTANTE, leer antes de correr nada):
-- Cada operacion que CREA un registro (solicitud, version de alcance,
-- estimacion, evaluacion, decision) vive en su PROPIO bloque "X.Y -- CREAR",
-- separado de cualquier bloque que seguira usando el identificador
-- devuelto. Un bloque "CREAR" se ejecuta UNA SOLA VEZ: termina justo
-- despues del EXEC de creacion, sin ningun IF/RETURN que dependa de un
-- identificador que ese mismo EXEC todavia no ha devuelto. El identificador
-- resultante se copia del resultset y se pega en el bloque "CONTINUAR"
-- correspondiente -- nunca se vuelve a ejecutar el bloque "CREAR" para
-- seguir probando, porque eso duplicaria el registro (nueva fila con
-- NumeroVersion/NumeroEvaluacion/NumeroDecision siguiente, no un error).
--
-- Cada bloque "CONTINUAR" declara al inicio TODAS las variables que
-- necesita (los UsuarioId de prueba y los identificadores pegados de
-- bloques anteriores) con un DECLARE ... = NULL y una guarda
-- IF (... IS NULL) RAISERROR+RETURN -- esa guarda protege contra olvidar
-- pegar un valor, NUNCA aparece en el mismo bloque que la creacion del
-- identificador que esta pidiendo.
--
-- Cada bloque se selecciona completo en SSMS (desde su encabezado
-- "-- BLOQUE N.M" hasta el separador del bloque siguiente) y se ejecuta de
-- una sola vez. No ejecutar linea por linea ni bloques a medias.
--
-- Orden de instalacion antes de correr este archivo:
--   1) 01_Schema_Y_StoredProcedures_Fase2A.sql   (si Fase 2A no esta instalada)
--      - o, si Fase 2A YA esta instalada y solo falta la ampliacion del
--        catalogo de transiciones: 03_Actualizacion_CambiarEstado_Fase2B.sql
--   2) 02_StoredProcedures_Fase2B.sql
--   3) Este archivo, bloque por bloque, en el orden en que aparecen.
--
-- Convencion:
--   -- [POSITIVA]  la operacion debe tener EXITO.
--   -- [NEGATIVA]  la operacion debe fallar con RAISERROR (>= 50000); se
--                  indica el mensaje esperado para verificarlo.
--
-- Seguridad: este script NO contiene ningun DELETE. No borra historial
-- (TI_SolicitudEstadosHistorial, TI_AuditoriaEventos, TI_AprobacionesAlcance,
-- TI_AprobacionesPrioridad). La ultima seccion (verificacion) es solo un
-- SELECT.
-- ============================================================================


-- ============================================================================
-- BLOQUE 1 -- CREAR la solicitud base de pruebas
-- Ejecutar una sola vez. Copiar el SolicitudId que imprime el EXEC: se pega
-- en todos los bloques "CONTINUAR" siguientes. No volver a ejecutar este
-- bloque para la misma corrida de pruebas (crearia una solicitud distinta).
-- ============================================================================

DECLARE @UsrSolicitante INT = NULL;  -- <<< PONER AQUI un UsuarioId real
DECLARE @UsrJefaturaTI  INT = NULL;  -- <<< PONER AQUI un UsuarioId real con rol JEFATURA_TI activo

IF (@UsrSolicitante IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN
    RAISERROR('Completar los UsuarioId de prueba al inicio del bloque antes de ejecutar.', 16, 1);
    RETURN;
END

DECLARE @TipoSolicitudId INT = (SELECT TOP 1 TipoSolicitudId FROM dbo.TI_TiposSolicitud WHERE Activo = 1 ORDER BY Orden);
IF (@TipoSolicitudId IS NULL)
BEGIN RAISERROR('No hay ningun TI_TiposSolicitud activo en esta base; no se puede continuar.', 16, 1); RETURN; END

-- [POSITIVA] Crear solicitud (Fase 2A, sin cambios).
EXEC dbo.sp_TI_Solicitudes_Crear
    @Titulo = N'PRUEBA_FASE2B_AUTOMATIZADA - no usar en produccion',
    @DescripcionNecesidad = N'Solicitud de prueba funcional para Fase 2B',
    @TipoSolicitudId = @TipoSolicitudId,
    @SolicitanteUsuarioId = @UsrSolicitante,
    @CreadaPorUsuarioId = @UsrSolicitante;
-- El SELECT final del SP regresa SolicitudId y Folio: copiar el SolicitudId.


-- ============================================================================
-- BLOQUE 2 -- CONTINUAR: transicion ENVIADA -> EN_REVISION, y una transicion
-- invalida. No crea ningun registro nuevo; usa el SolicitudId del Bloque 1.
-- ============================================================================

DECLARE @SolicitudId BIGINT = NULL;  -- <<< PEGAR AQUI el SolicitudId del Bloque 1
DECLARE @UsrJefaturaTI INT = NULL;   -- <<< PONER AQUI el mismo UsuarioId JEFATURA_TI del Bloque 1

IF (@SolicitudId IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN RAISERROR('Completar @SolicitudId y @UsrJefaturaTI antes de ejecutar este bloque.', 16, 1); RETURN; END

-- [POSITIVA]
EXEC dbo.sp_TI_Solicitudes_CambiarEstado @SolicitudId = @SolicitudId, @EstadoNuevoCodigo = 'EN_REVISION', @usuarioId = @UsrJefaturaTI;

-- [NEGATIVA] transicion no permitida desde EN_REVISION directo a PRIORIZADA
EXEC dbo.sp_TI_Solicitudes_CambiarEstado @SolicitudId = @SolicitudId, @EstadoNuevoCodigo = 'PRIORIZADA', @usuarioId = @UsrJefaturaTI;
-- Esperado: RAISERROR 'La transicion de estado solicitada no esta permitida'


-- ============================================================================
-- BLOQUE 3.1 -- CREAR la version de alcance (requiere que el Bloque 2 ya
-- haya dejado la solicitud en EN_REVISION).
-- Ejecutar una sola vez. Copiar el VersionAlcanceId que imprime el EXEC: se
-- pega en el Bloque 3.2. No volver a ejecutar este bloque para la misma
-- version de prueba.
-- ============================================================================

DECLARE @SolicitudId BIGINT = NULL; -- <<< PEGAR AQUI el SolicitudId del Bloque 1
DECLARE @UsrJefaturaTI INT = NULL;  -- <<< PONER AQUI el mismo UsuarioId JEFATURA_TI

IF (@SolicitudId IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN RAISERROR('Completar @SolicitudId y @UsrJefaturaTI antes de ejecutar este bloque.', 16, 1); RETURN; END

DECLARE @JsonFuncionalidades NVARCHAR(MAX) = N'[
    {"Codigo":"F01","Nombre":"Funcionalidad 1","Descripcion":"Desc 1","ReglasNegocio":"R1","CriteriosAceptacion":"C1","PrioridadInterna":1,"Complejidad":"MEDIA","Orden":1},
    {"Codigo":"F02","Nombre":"Funcionalidad 2","Descripcion":"Desc 2","ReglasNegocio":"R2","CriteriosAceptacion":"C2","PrioridadInterna":2,"Complejidad":"ALTA","Orden":2}
]';

-- [POSITIVA] crear version de alcance con 2 funcionalidades (tambien mueve la
-- solicitud a DEFINICION_ALCANCE automaticamente).
EXEC dbo.sp_TI_VersionesAlcance_Crear
    @SolicitudId = @SolicitudId,
    @Objetivo = N'Objetivo de prueba',
    @AlcanceIncluido = N'Incluye A y B',
    @usuarioId = @UsrJefaturaTI,
    @json = @JsonFuncionalidades;
-- Copiar el VersionAlcanceId que regresa.


-- ============================================================================
-- BLOQUE 3.2 -- CONTINUAR: version en curso (negativa), enviar a aprobacion,
-- aprobaciones duales. No crea ninguna version nueva; usa el VersionAlcanceId
-- del Bloque 3.1.
-- ============================================================================

DECLARE @SolicitudId BIGINT = NULL;      -- <<< PEGAR AQUI el SolicitudId del Bloque 1
DECLARE @VersionAlcanceId BIGINT = NULL; -- <<< PEGAR AQUI el VersionAlcanceId del Bloque 3.1
DECLARE @UsrSolicitante INT = NULL;      -- <<< PONER AQUI el mismo UsuarioId solicitante del Bloque 1
DECLARE @UsrJefaturaTI INT = NULL;       -- <<< PONER AQUI el mismo UsuarioId JEFATURA_TI
DECLARE @UsrAjeno INT = NULL;            -- <<< PONER AQUI un UsuarioId real SIN rol SOLICITANTE/JEFATURA_TI sobre esta version

IF (@SolicitudId IS NULL OR @VersionAlcanceId IS NULL OR @UsrSolicitante IS NULL OR @UsrJefaturaTI IS NULL OR @UsrAjeno IS NULL)
BEGIN RAISERROR('Completar @SolicitudId, @VersionAlcanceId y los 3 UsuarioId antes de ejecutar este bloque.', 16, 1); RETURN; END

-- [NEGATIVA] crear una segunda version de alcance BORRADOR mientras la
-- primera sigue BORRADOR/PENDIENTE_APROBACION. No crea nada (el SP rechaza
-- antes del INSERT); seguro de re-ejecutar.
DECLARE @JsonFuncionalidadExtra NVARCHAR(MAX) = N'[{"Codigo":"F03","Nombre":"F3","Descripcion":"d","ReglasNegocio":"r","CriteriosAceptacion":"c","PrioridadInterna":1,"Complejidad":"BAJA","Orden":1}]';
EXEC dbo.sp_TI_VersionesAlcance_Crear
    @SolicitudId = @SolicitudId, @Objetivo = N'x', @AlcanceIncluido = N'x', @usuarioId = @UsrJefaturaTI,
    @json = @JsonFuncionalidadExtra;
-- Esperado: RAISERROR 'Ya existe una version de alcance en curso para esta solicitud'

-- [POSITIVA] enviar a aprobacion.
EXEC dbo.sp_TI_VersionesAlcance_EnviarAprobacion @VersionAlcanceId = @VersionAlcanceId, @usuarioId = @UsrJefaturaTI;

-- [NEGATIVA] aprobar con un usuario que no es ni el solicitante ni JEFATURA_TI.
EXEC dbo.sp_TI_AprobacionesAlcance_Registrar
    @VersionAlcanceId = @VersionAlcanceId, @TipoAprobador = 'SOLICITANTE', @EstadoDecision = 'APROBADA', @usuarioId = @UsrAjeno;
-- Esperado: RAISERROR 'El usuario no es el solicitante de esta version de alcance'

-- [POSITIVA] lado SOLICITANTE aprueba.
EXEC dbo.sp_TI_AprobacionesAlcance_Registrar
    @VersionAlcanceId = @VersionAlcanceId, @TipoAprobador = 'SOLICITANTE', @EstadoDecision = 'APROBADA', @usuarioId = @UsrSolicitante;

-- [NEGATIVA] intentar volver a aprobar el mismo lado (ya resuelto).
EXEC dbo.sp_TI_AprobacionesAlcance_Registrar
    @VersionAlcanceId = @VersionAlcanceId, @TipoAprobador = 'SOLICITANTE', @EstadoDecision = 'APROBADA', @usuarioId = @UsrSolicitante;
-- Esperado: RAISERROR 'Esta aprobacion ya fue resuelta anteriormente y no puede modificarse'

-- [POSITIVA] lado JEFATURA_TI aprueba -> version pasa a APROBADA, solicitud
-- avanza automaticamente a EVALUACION_TECNICA.
EXEC dbo.sp_TI_AprobacionesAlcance_Registrar
    @VersionAlcanceId = @VersionAlcanceId, @TipoAprobador = 'JEFATURA_TI', @EstadoDecision = 'APROBADA', @usuarioId = @UsrJefaturaTI;

-- Verificacion de resultado esperado:
SELECT EstadoVersion FROM dbo.TI_VersionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId; -- esperado: APROBADA
SELECT es.Codigo FROM dbo.TI_Solicitudes s INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId WHERE s.SolicitudId = @SolicitudId; -- esperado: EVALUACION_TECNICA

-- IMPORTANTE: una vez ejecutado con exito, no volver a ejecutar este
-- bloque 3.2 sobre la misma version -- la segunda llamada a cada EXEC de
-- aprobacion caeria en la rama [NEGATIVA] "ya fue resuelta" y la aprobacion
-- de JEFATURA_TI (ultima linea positiva) fallaria igual por la misma razon.


-- ============================================================================
-- BLOQUE 3B -- Caso de RECHAZO de alcance (opcional, en una solicitud de
-- prueba DISTINTA e independiente). Para prepararlo: repetir el Bloque 1 y
-- el Bloque 3.1 (crear solicitud + crear version + enviar a aprobacion,
-- usando el Bloque 3.2 solo hasta el EXEC de EnviarAprobacion) con una
-- solicitud nueva, y copiar su VersionAlcanceId como @VersionAlcanceId2.
-- Este bloque NO crea nada; solo registra un rechazo sobre esa version ya
-- existente y PENDIENTE_APROBACION.
-- ============================================================================

DECLARE @VersionAlcanceId2 BIGINT = NULL; -- <<< PEGAR AQUI el VersionAlcanceId de la solicitud de prueba del rechazo
DECLARE @UsrJefaturaTI INT = NULL;        -- <<< PONER AQUI el UsuarioId JEFATURA_TI

IF (@VersionAlcanceId2 IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN RAISERROR('Completar @VersionAlcanceId2 y @UsrJefaturaTI antes de ejecutar este bloque.', 16, 1); RETURN; END

-- [NEGATIVA->estado] rechazo de una de las dos aprobaciones.
EXEC dbo.sp_TI_AprobacionesAlcance_Registrar
    @VersionAlcanceId = @VersionAlcanceId2, @TipoAprobador = 'JEFATURA_TI', @EstadoDecision = 'RECHAZADA', @Comentario = N'Motivo de prueba', @usuarioId = @UsrJefaturaTI;
SELECT EstadoVersion FROM dbo.TI_VersionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId2; -- esperado: RECHAZADA


-- ============================================================================
-- BLOQUE 4.1 -- NEGATIVAS de estimacion (detalle incompleto, duplicado,
-- foraneo). Ninguna de estas llamadas inserta una fila (el SP rechaza antes
-- del INSERT): seguro de re-ejecutar cuantas veces sea necesario. Requiere
-- la version de alcance APROBADA del Bloque 3.2 (solicitud en
-- EVALUACION_TECNICA).
-- ============================================================================

DECLARE @SolicitudId BIGINT = NULL;      -- <<< PEGAR AQUI el SolicitudId del Bloque 1
DECLARE @VersionAlcanceId BIGINT = NULL; -- <<< PEGAR AQUI el VersionAlcanceId aprobado del Bloque 3.1
DECLARE @UsrJefaturaTI INT = NULL;       -- <<< PONER AQUI el UsuarioId JEFATURA_TI

IF (@SolicitudId IS NULL OR @VersionAlcanceId IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN RAISERROR('Completar @SolicitudId, @VersionAlcanceId y @UsrJefaturaTI antes de ejecutar este bloque.', 16, 1); RETURN; END

DECLARE @FuncionalidadId1 BIGINT = (SELECT TOP 1 FuncionalidadId FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId = @VersionAlcanceId AND Codigo = 'F01');
DECLARE @FuncionalidadId2 BIGINT = (SELECT TOP 1 FuncionalidadId FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId = @VersionAlcanceId AND Codigo = 'F02');
DECLARE @FuncionalidadForanea BIGINT = (SELECT TOP 1 FuncionalidadId FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId <> @VersionAlcanceId); -- de OTRA version, si existe alguna en la base

IF (@FuncionalidadId1 IS NULL OR @FuncionalidadId2 IS NULL)
BEGIN RAISERROR('No se encontraron las funcionalidades F01/F02 del Bloque 3.1 bajo este VersionAlcanceId.', 16, 1); RETURN; END

-- [NEGATIVA] detalle incompleto (falta F02).
DECLARE @JsonDetalleIncompleto NVARCHAR(MAX) = N'[{"FuncionalidadId":' + CAST(@FuncionalidadId1 AS NVARCHAR(20)) + N',"HorasAnalisis":1,"HorasDesarrollo":1,"HorasPruebas":1,"HorasDespliegue":1}]';
EXEC dbo.sp_TI_Estimaciones_Crear
    @SolicitudId = @SolicitudId, @VersionAlcanceId = @VersionAlcanceId, @RegistradaPorUsuarioId = @UsrJefaturaTI,
    @json = @JsonDetalleIncompleto;
-- Esperado: RAISERROR 'Falta el detalle de estimacion de alguna funcionalidad activa de la version'

-- [NEGATIVA] detalle duplicado (F01 dos veces).
DECLARE @JsonDetalleDuplicado NVARCHAR(MAX) =
    N'[{"FuncionalidadId":' + CAST(@FuncionalidadId1 AS NVARCHAR(20)) + N',"HorasAnalisis":1,"HorasDesarrollo":1,"HorasPruebas":1,"HorasDespliegue":1},' +
    N'{"FuncionalidadId":' + CAST(@FuncionalidadId1 AS NVARCHAR(20)) + N',"HorasAnalisis":1,"HorasDesarrollo":1,"HorasPruebas":1,"HorasDespliegue":1},' +
    N'{"FuncionalidadId":' + CAST(@FuncionalidadId2 AS NVARCHAR(20)) + N',"HorasAnalisis":1,"HorasDesarrollo":1,"HorasPruebas":1,"HorasDespliegue":1}]';
EXEC dbo.sp_TI_Estimaciones_Crear
    @SolicitudId = @SolicitudId, @VersionAlcanceId = @VersionAlcanceId, @RegistradaPorUsuarioId = @UsrJefaturaTI,
    @json = @JsonDetalleDuplicado;
-- Esperado: RAISERROR 'El detalle de estimacion contiene funcionalidades duplicadas'

-- [NEGATIVA] funcionalidad de otra version (solo se puede probar si existe
-- al menos una FuncionalidadId de otra version en la base; si
-- @FuncionalidadForanea sale NULL, omitir esta prueba puntual -- no
-- inventar un id).
IF (@FuncionalidadForanea IS NOT NULL)
BEGIN
    DECLARE @JsonDetalleForaneo NVARCHAR(MAX) =
        N'[{"FuncionalidadId":' + CAST(@FuncionalidadId1 AS NVARCHAR(20)) + N',"HorasAnalisis":1,"HorasDesarrollo":1,"HorasPruebas":1,"HorasDespliegue":1},' +
        N'{"FuncionalidadId":' + CAST(@FuncionalidadId2 AS NVARCHAR(20)) + N',"HorasAnalisis":1,"HorasDesarrollo":1,"HorasPruebas":1,"HorasDespliegue":1},' +
        N'{"FuncionalidadId":' + CAST(@FuncionalidadForanea AS NVARCHAR(20)) + N',"HorasAnalisis":1,"HorasDesarrollo":1,"HorasPruebas":1,"HorasDespliegue":1}]';
    EXEC dbo.sp_TI_Estimaciones_Crear
        @SolicitudId = @SolicitudId, @VersionAlcanceId = @VersionAlcanceId, @RegistradaPorUsuarioId = @UsrJefaturaTI,
        @json = @JsonDetalleForaneo;
    -- Esperado: RAISERROR 'El detalle incluye funcionalidades que no pertenecen a la version de alcance indicada'
END


-- ============================================================================
-- BLOQUE 4.2 -- CREAR la estimacion valida. Ejecutar una sola vez. Copiar el
-- EstimacionId que imprime el EXEC: se pega en el Bloque 4.3. No volver a
-- ejecutar este bloque para la misma estimacion de prueba.
-- ============================================================================

DECLARE @SolicitudId BIGINT = NULL;      -- <<< PEGAR AQUI el SolicitudId del Bloque 1
DECLARE @VersionAlcanceId BIGINT = NULL; -- <<< PEGAR AQUI el VersionAlcanceId del Bloque 3.1
DECLARE @UsrJefaturaTI INT = NULL;       -- <<< PONER AQUI el UsuarioId JEFATURA_TI

IF (@SolicitudId IS NULL OR @VersionAlcanceId IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN RAISERROR('Completar @SolicitudId, @VersionAlcanceId y @UsrJefaturaTI antes de ejecutar este bloque.', 16, 1); RETURN; END

DECLARE @FuncionalidadId1 BIGINT = (SELECT TOP 1 FuncionalidadId FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId = @VersionAlcanceId AND Codigo = 'F01');
DECLARE @FuncionalidadId2 BIGINT = (SELECT TOP 1 FuncionalidadId FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId = @VersionAlcanceId AND Codigo = 'F02');

IF (@FuncionalidadId1 IS NULL OR @FuncionalidadId2 IS NULL)
BEGIN RAISERROR('No se encontraron las funcionalidades F01/F02 del Bloque 3.1 bajo este VersionAlcanceId.', 16, 1); RETURN; END

-- [POSITIVA] estimacion completa y valida.
DECLARE @JsonDetalleValido NVARCHAR(MAX) =
    N'[{"FuncionalidadId":' + CAST(@FuncionalidadId1 AS NVARCHAR(20)) + N',"HorasAnalisis":4,"HorasDesarrollo":10,"HorasPruebas":3,"HorasDespliegue":1},' +
    N'{"FuncionalidadId":' + CAST(@FuncionalidadId2 AS NVARCHAR(20)) + N',"HorasAnalisis":2,"HorasDesarrollo":6,"HorasPruebas":2,"HorasDespliegue":1}]';
EXEC dbo.sp_TI_Estimaciones_Crear
    @SolicitudId = @SolicitudId, @VersionAlcanceId = @VersionAlcanceId, @RegistradaPorUsuarioId = @UsrJefaturaTI,
    @json = @JsonDetalleValido;
-- Copiar el EstimacionId que regresa.


-- ============================================================================
-- BLOQUE 4.3 -- CONTINUAR: verificar HorasTotal, validar la estimacion,
-- negativa de re-validacion. No crea ninguna estimacion nueva; usa el
-- EstimacionId del Bloque 4.2.
-- ============================================================================

DECLARE @EstimacionId BIGINT = NULL; -- <<< PEGAR AQUI el EstimacionId del Bloque 4.2
DECLARE @UsrJefaturaTI INT = NULL;   -- <<< PONER AQUI el UsuarioId JEFATURA_TI

IF (@EstimacionId IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN RAISERROR('Completar @EstimacionId y @UsrJefaturaTI antes de ejecutar este bloque.', 16, 1); RETURN; END

-- Verificacion: HorasTotal debe ser la suma de las horas de ambas filas
-- (4+10+3+1 + 2+6+2+1 = 29), leida de la columna calculada:
SELECT HorasAnalisis, HorasDesarrollo, HorasPruebas, HorasDespliegue, HorasTotal FROM dbo.TI_Estimaciones WHERE EstimacionId = @EstimacionId;

-- [POSITIVA] validar la estimacion (mueve la solicitud a PENDIENTE_PRIORIZACION).
EXEC dbo.sp_TI_Estimaciones_Validar @EstimacionId = @EstimacionId, @usuarioId = @UsrJefaturaTI;

-- [NEGATIVA] volver a validar la misma estimacion.
EXEC dbo.sp_TI_Estimaciones_Validar @EstimacionId = @EstimacionId, @usuarioId = @UsrJefaturaTI;
-- Esperado: RAISERROR 'La estimacion ya fue validada o sustituida anteriormente'


-- ============================================================================
-- BLOQUE 5.1 -- CREAR la evaluacion de prioridad con las 6 puntuaciones.
-- Ejecutar una sola vez. Copiar el EvaluacionPrioridadId que imprime el
-- EXEC: se pega en el Bloque 5.2. No volver a ejecutar este bloque para la
-- misma evaluacion de prueba. Requiere la estimacion VALIDADA del
-- Bloque 4.3 (solicitud en PENDIENTE_PRIORIZACION).
-- ============================================================================

DECLARE @SolicitudId BIGINT = NULL;      -- <<< PEGAR AQUI el SolicitudId del Bloque 1
DECLARE @VersionAlcanceId BIGINT = NULL; -- <<< PEGAR AQUI el VersionAlcanceId del Bloque 3.1
DECLARE @UsrJefaturaTI INT = NULL;       -- <<< PONER AQUI el UsuarioId JEFATURA_TI

IF (@SolicitudId IS NULL OR @VersionAlcanceId IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN RAISERROR('Completar @SolicitudId, @VersionAlcanceId y @UsrJefaturaTI antes de ejecutar este bloque.', 16, 1); RETURN; END

DECLARE @JsonCriterios NVARCHAR(MAX) = (
    SELECT N'[' + STRING_AGG(CONCAT(N'{"CriterioPrioridadId":', CriterioPrioridadId, N',"Puntuacion":3,"Comentario":"ok"}'), N',') + N']'
    FROM dbo.TI_CriteriosPrioridad WHERE Activo = 1
);
-- Si la base no tiene exactamente 6 criterios activos con pesos que suman
-- 100, el SP rechazara este bloque entero -- verificar el catalogo primero,
-- no es un error de este script.
IF (@JsonCriterios IS NULL)
BEGIN RAISERROR('No hay criterios activos en TI_CriteriosPrioridad; verificar el catalogo antes de continuar.', 16, 1); RETURN; END

-- [POSITIVA] crear evaluacion con las 6 puntuaciones.
EXEC dbo.sp_TI_EvaluacionesPrioridad_Crear
    @SolicitudId = @SolicitudId, @VersionAlcanceId = @VersionAlcanceId, @EvaluadaPorUsuarioId = @UsrJefaturaTI, @json = @JsonCriterios;
-- Copiar el EvaluacionPrioridadId que regresa.


-- ============================================================================
-- BLOQUE 5.2 -- CONTINUAR: editar en BORRADOR, negativa de rango, completar,
-- verificar formula, negativas tras completar. No crea ninguna evaluacion
-- nueva; usa el EvaluacionPrioridadId del Bloque 5.1.
-- ============================================================================

DECLARE @EvaluacionPrioridadId BIGINT = NULL; -- <<< PEGAR AQUI el EvaluacionPrioridadId del Bloque 5.1
DECLARE @UsrJefaturaTI INT = NULL;            -- <<< PONER AQUI el UsuarioId JEFATURA_TI

IF (@EvaluacionPrioridadId IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN RAISERROR('Completar @EvaluacionPrioridadId y @UsrJefaturaTI antes de ejecutar este bloque.', 16, 1); RETURN; END

DECLARE @PrimerCriterioId INT = (SELECT TOP 1 CriterioPrioridadId FROM dbo.TI_CriteriosPrioridad WHERE Activo = 1 ORDER BY CriterioPrioridadId);

-- [POSITIVA] editar una puntuacion mientras esta en BORRADOR.
EXEC dbo.sp_TI_EvaluacionesPrioridad_RegistrarDetalle
    @EvaluacionPrioridadId = @EvaluacionPrioridadId, @CriterioPrioridadId = @PrimerCriterioId, @Puntuacion = 5, @Comentario = N'ajustado', @usuarioId = @UsrJefaturaTI;

-- [NEGATIVA] puntuacion fuera de rango. No modifica nada (el SP rechaza
-- antes del UPDATE); segura de re-ejecutar.
EXEC dbo.sp_TI_EvaluacionesPrioridad_RegistrarDetalle
    @EvaluacionPrioridadId = @EvaluacionPrioridadId, @CriterioPrioridadId = @PrimerCriterioId, @Puntuacion = 9, @usuarioId = @UsrJefaturaTI;
-- Esperado: RAISERROR 'La puntuacion debe estar entre 1 y 5'

-- [POSITIVA] completar evaluacion (calcula PuntajeTotal). A partir de aqui
-- la evaluacion queda COMPLETA; no se puede volver a editar ni completar
-- (ver las dos negativas de abajo), asi que esta linea tambien debe
-- ejecutarse una sola vez por evaluacion.
EXEC dbo.sp_TI_EvaluacionesPrioridad_Completar @EvaluacionPrioridadId = @EvaluacionPrioridadId, @usuarioId = @UsrJefaturaTI;

-- Verificacion manual de la formula aprobada para esta fase
-- (SUM(Puntuacion/5*PesoAplicado), ver cabecera de 02_StoredProcedures_Fase2B.sql
-- sobre la discrepancia documentada y no confirmada):
SELECT PuntajeTotal FROM dbo.TI_EvaluacionesPrioridad WHERE EvaluacionPrioridadId = @EvaluacionPrioridadId;
SELECT CriterioPrioridadId, Puntuacion, PesoAplicado FROM dbo.TI_EvaluacionPrioridadDetalle WHERE EvaluacionPrioridadId = @EvaluacionPrioridadId;
-- Calcular a mano: SUM(Puntuacion/5.0*PesoAplicado) de las 6 filas y comparar
-- contra PuntajeTotal -- deben coincidir exactamente.

-- [NEGATIVA] editar detalle de una evaluacion ya COMPLETA.
EXEC dbo.sp_TI_EvaluacionesPrioridad_RegistrarDetalle
    @EvaluacionPrioridadId = @EvaluacionPrioridadId, @CriterioPrioridadId = @PrimerCriterioId, @Puntuacion = 1, @usuarioId = @UsrJefaturaTI;
-- Esperado: RAISERROR 'La evaluacion ya fue completada y no admite modificaciones'

-- [NEGATIVA] volver a completar la misma evaluacion.
EXEC dbo.sp_TI_EvaluacionesPrioridad_Completar @EvaluacionPrioridadId = @EvaluacionPrioridadId, @usuarioId = @UsrJefaturaTI;
-- Esperado: RAISERROR 'La evaluacion ya fue completada anteriormente'


-- ============================================================================
-- BLOQUE 6.1 -- NEGATIVAS de PrioridadId sobre la decision PRIORIZAR.
-- Ninguna de estas llamadas inserta una fila (el SP rechaza antes del
-- INSERT): seguro de re-ejecutar. Requiere la evaluacion COMPLETA del
-- Bloque 5.2.
-- ============================================================================

DECLARE @SolicitudId BIGINT = NULL;            -- <<< PEGAR AQUI el SolicitudId del Bloque 1
DECLARE @EvaluacionPrioridadId BIGINT = NULL;  -- <<< PEGAR AQUI el EvaluacionPrioridadId del Bloque 5.1
DECLARE @UsrJefaturaTI INT = NULL;             -- <<< PONER AQUI el UsuarioId JEFATURA_TI

IF (@SolicitudId IS NULL OR @EvaluacionPrioridadId IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN RAISERROR('Completar @SolicitudId, @EvaluacionPrioridadId y @UsrJefaturaTI antes de ejecutar este bloque.', 16, 1); RETURN; END

DECLARE @PrioridadIdInexistente INT = (SELECT ISNULL(MAX(PrioridadId), 0) + 99999 FROM dbo.TI_Prioridades); -- garantizado inexistente

-- [NEGATIVA] PRIORIZAR sin PrioridadId.
EXEC dbo.sp_TI_DecisionesPrioridad_Crear
    @SolicitudId = @SolicitudId, @EvaluacionPrioridadId = @EvaluacionPrioridadId, @TipoDecision = 'PRIORIZAR', @Motivo = N'prueba', @usuarioId = @UsrJefaturaTI;
-- Esperado: RAISERROR 'La decision PRIORIZAR requiere especificar una prioridad'

-- [NEGATIVA] PRIORIZAR con PrioridadId inexistente.
EXEC dbo.sp_TI_DecisionesPrioridad_Crear
    @SolicitudId = @SolicitudId, @EvaluacionPrioridadId = @EvaluacionPrioridadId, @TipoDecision = 'PRIORIZAR', @PrioridadId = @PrioridadIdInexistente, @Motivo = N'prueba', @usuarioId = @UsrJefaturaTI;
-- Esperado: RAISERROR 'La prioridad especificada no existe'


-- ============================================================================
-- BLOQUE 6.2 -- CREAR la decision PRIORIZAR con PrioridadId real. Ejecutar
-- una sola vez. Copiar el DecisionPrioridadId que imprime el EXEC: se pega
-- en el Bloque 6.3. No volver a ejecutar este bloque para la misma decision
-- de prueba.
-- ============================================================================

DECLARE @SolicitudId BIGINT = NULL;            -- <<< PEGAR AQUI el SolicitudId del Bloque 1
DECLARE @EvaluacionPrioridadId BIGINT = NULL;  -- <<< PEGAR AQUI el EvaluacionPrioridadId del Bloque 5.1
DECLARE @UsrJefaturaTI INT = NULL;             -- <<< PONER AQUI el UsuarioId JEFATURA_TI

IF (@SolicitudId IS NULL OR @EvaluacionPrioridadId IS NULL OR @UsrJefaturaTI IS NULL)
BEGIN RAISERROR('Completar @SolicitudId, @EvaluacionPrioridadId y @UsrJefaturaTI antes de ejecutar este bloque.', 16, 1); RETURN; END

DECLARE @PrioridadIdValida INT = (SELECT TOP 1 PrioridadId FROM dbo.TI_Prioridades ORDER BY PrioridadId);
IF (@PrioridadIdValida IS NULL)
BEGIN RAISERROR('No hay ninguna fila en TI_Prioridades en esta base; no se puede continuar.', 16, 1); RETURN; END

-- [POSITIVA] PRIORIZAR con PrioridadId real.
EXEC dbo.sp_TI_DecisionesPrioridad_Crear
    @SolicitudId = @SolicitudId, @EvaluacionPrioridadId = @EvaluacionPrioridadId, @TipoDecision = 'PRIORIZAR', @PrioridadId = @PrioridadIdValida, @Motivo = N'prueba', @usuarioId = @UsrJefaturaTI;
-- Copiar el DecisionPrioridadId que regresa. (@PrioridadIdValida usada aqui
-- se vuelve a resolver igual -- TOP 1 por PrioridadId -- en el Bloque 6.3
-- para la verificacion final; en una base sin cambios de catalogo entre
-- bloques da el mismo valor.)


-- ============================================================================
-- BLOQUE 6.3 -- CONTINUAR: aprobacion dual, negativas de autorizacion,
-- verificacion final. No crea ninguna decision nueva; usa el
-- DecisionPrioridadId del Bloque 6.2.
-- ============================================================================

DECLARE @SolicitudId BIGINT = NULL;              -- <<< PEGAR AQUI el SolicitudId del Bloque 1
DECLARE @DecisionPrioridadId BIGINT = NULL;      -- <<< PEGAR AQUI el DecisionPrioridadId del Bloque 6.2
DECLARE @UsrJefaturaTI INT = NULL;               -- <<< PONER AQUI el UsuarioId JEFATURA_TI
DECLARE @UsrGerenciaPlaneacion INT = NULL;       -- <<< PONER AQUI un UsuarioId real con rol GERENCIA_PLANEACION activo
DECLARE @UsrAjeno INT = NULL;                    -- <<< PONER AQUI un UsuarioId real sin esos 2 roles

IF (@SolicitudId IS NULL OR @DecisionPrioridadId IS NULL OR @UsrJefaturaTI IS NULL OR @UsrGerenciaPlaneacion IS NULL OR @UsrAjeno IS NULL)
BEGIN RAISERROR('Completar todas las variables de este bloque antes de ejecutar.', 16, 1); RETURN; END

DECLARE @PrioridadIdValida INT = (SELECT TOP 1 PrioridadId FROM dbo.TI_Prioridades ORDER BY PrioridadId);

-- [NEGATIVA] abrir una nueva version de alcance mientras esta decision
-- sigue PENDIENTE (reutilizar el Bloque 3.1 con este mismo @SolicitudId).
-- Esperado: RAISERROR 'Existe una decision de prioridad pendiente de
-- aprobacion...'. No ejecutar esa prueba desde este bloque; es una llamada
-- aparte al SP de la Seccion 3.1 que se deja documentada aqui como
-- recordatorio, no como parte de este bloque.

-- [NEGATIVA] aprobar con un usuario sin el rol que dice representar.
EXEC dbo.sp_TI_AprobacionesPrioridad_Registrar
    @DecisionPrioridadId = @DecisionPrioridadId, @TipoAprobador = 'JEFATURA_TI', @EstadoDecision = 'APROBADA', @usuarioId = @UsrAjeno;
-- Esperado: RAISERROR (el SP valida el rol real del usuario)

-- [POSITIVA] lado JEFATURA_TI aprueba.
EXEC dbo.sp_TI_AprobacionesPrioridad_Registrar
    @DecisionPrioridadId = @DecisionPrioridadId, @TipoAprobador = 'JEFATURA_TI', @EstadoDecision = 'APROBADA', @usuarioId = @UsrJefaturaTI;

-- [NEGATIVA] repetir el mismo lado.
EXEC dbo.sp_TI_AprobacionesPrioridad_Registrar
    @DecisionPrioridadId = @DecisionPrioridadId, @TipoAprobador = 'JEFATURA_TI', @EstadoDecision = 'APROBADA', @usuarioId = @UsrJefaturaTI;
-- Esperado: RAISERROR (aprobacion ya resuelta)

-- [POSITIVA] lado GERENCIA_PLANEACION aprueba -> decision CONFIRMADA,
-- TI_Solicitudes.PrioridadId/PuntajePrioridadActual actualizados, estado
-- final. Esta linea tambien es de ejecucion unica (la decision queda
-- CONFIRMADA y no admite mas aprobaciones).
EXEC dbo.sp_TI_AprobacionesPrioridad_Registrar
    @DecisionPrioridadId = @DecisionPrioridadId, @TipoAprobador = 'GERENCIA_PLANEACION', @EstadoDecision = 'APROBADA', @usuarioId = @UsrGerenciaPlaneacion;

-- Verificacion de resultado esperado:
SELECT EstadoDecision FROM dbo.TI_DecisionesPrioridad WHERE DecisionPrioridadId = @DecisionPrioridadId; -- esperado: CONFIRMADA
SELECT s.PrioridadId, p.Codigo, es.Codigo AS EstadoSolicitudCodigo
FROM dbo.TI_Solicitudes s
INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
LEFT JOIN dbo.TI_Prioridades p ON p.PrioridadId = s.PrioridadId
WHERE s.SolicitudId = @SolicitudId;
-- esperado: PrioridadId = @PrioridadIdValida, EstadoSolicitudCodigo = 'PRIORIZADA'

SELECT * FROM dbo.TI_SolicitudEstadosHistorial WHERE SolicitudId = @SolicitudId ORDER BY FechaCambio;
-- esperado: una fila de historial por cada transicion de los bloques 1-6
-- (nunca se sobreescribe ni se borra, es registro historico puro)


-- ============================================================================
-- SECCION 7 -- Pruebas de CONCURRENCIA (requieren 2 sesiones de SSMS)
-- No se pueden automatizar en un solo lote: hay que abrir dos pestanas y
-- coordinar manualmente el orden de ejecucion como se indica. Ninguna de
-- estas 3 pruebas crea un registro de prueba nuevo por si misma: reutilizan
-- identificadores ya creados en bloques anteriores (o, en 7.3, crean una
-- solicitud/version nueva exclusivamente para esa prueba puntual, siguiendo
-- el Bloque 1 + Bloque 3.1 de este mismo archivo).
-- ============================================================================
--
-- 7.1 Dos aprobaciones simultaneas del MISMO lado sobre la misma version de
--     alcance (p.ej. dos clics casi al mismo tiempo en "Aprobar JEFATURA_TI"):
--     - Sesion A: BEGIN TRANSACTION; EXEC sp_getapplock @Resource=<VersionAlcanceId>,
--       @LockMode='Exclusive', @LockOwner='Transaction'; -- (no hacer COMMIT todavia)
--     - Sesion B: EXEC dbo.sp_TI_AprobacionesAlcance_Registrar ... (mismo VersionAlcanceId, mismo TipoAprobador)
--       -> debe quedar BLOQUEADA esperando el lock de la Sesion A.
--     - Sesion A: COMMIT TRANSACTION; (libera el lock)
--     - Sesion B: su EXEC ahora continua y, como el estado ya fue resuelto por
--       la logica de negocio (si A tambien llamo al SP real en lugar de solo
--       tomar el lock de prueba), debe fallar con 'Esta aprobacion ya fue
--       resuelta anteriormente y no puede modificarse'. Si A solo tomo el
--       lock de prueba sin resolver nada, B debe completar normalmente
--       despues de esperar -- esto confirma que el lock serializa, no que
--       bloquea incorrectamente para siempre.
--
-- 7.2 Misma prueba sobre sp_TI_EvaluacionesPrioridad_RegistrarDetalle /
--     _Completar: Sesion A deja una transaccion abierta con el applock sobre
--     el mismo EvaluacionPrioridadId (sin commitear), Sesion B ejecuta
--     _RegistrarDetalle o _Completar sobre esa misma evaluacion y debe
--     quedar esperando hasta que A libere.
--
-- 7.3 Generacion de NumeroVersion/NumeroEvaluacion/NumeroDecision: Sesion A y
--     Sesion B ejecutan CASI al mismo tiempo (una inmediatamente despues de
--     la otra, sin esperar resultado) el Bloque 3.1 (sp_TI_VersionesAlcance_Crear)
--     sobre la MISMA SolicitudId recien creada sin version previa. Resultado
--     esperado: una de las dos debe fallar con 'Ya existe una version de
--     alcance en curso para esta solicitud' (el chequeo de "version en
--     curso" tambien esta protegido por el mismo applock de @SolicitudId)
--     -- nunca deben quedar dos filas con el mismo NumeroVersion para la
--     misma solicitud. Verificar con:
--     SELECT NumeroVersion, COUNT(*) FROM dbo.TI_VersionesAlcance WHERE SolicitudId = @SolicitudId GROUP BY NumeroVersion HAVING COUNT(*) > 1;
--     -- debe regresar 0 filas.
--
-- Estas 3 pruebas (7.1, 7.2, 7.3) son las que EXPLICITAMENTE requieren dos
-- sesiones concurrentes reales; todo lo demas en este archivo (Bloques 1 a
-- 6.3) es secuencial y se puede automatizar en un runner de pruebas de
-- integracion contra la base de desarrollo.


-- ============================================================================
-- SECCION 8 -- Verificacion de limpieza (sin DELETE)
-- ============================================================================
-- Este bloque NO borra nada -- solo permite localizar la solicitud de
-- prueba creada por el Bloque 1 para que el responsable decida manualmente
-- si la conserva (recomendado, para dejar rastro de la corrida) o la limpia
-- por su cuenta en otra sesion.

SELECT SolicitudId, Folio FROM dbo.TI_Solicitudes WHERE Titulo = N'PRUEBA_FASE2B_AUTOMATIZADA - no usar en produccion';
