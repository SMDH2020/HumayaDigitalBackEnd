-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Fase 2B (definicion de alcance,
-- estimacion y priorizacion). Script preparado para REVISION Y EJECUCION
-- MANUAL en el entorno de desarrollo. No se ha ejecutado contra ningun
-- servidor durante el desarrollo.
--
-- No crea tablas ni columnas: usa exclusivamente las 29 tablas TI_* y las
-- 2 vistas vw_TI_* ya existentes y validadas. Todos los SP viven en el
-- esquema dbo (mismo esquema de las tablas), con el prefijo sp_TI_, igual
-- que los de la Fase 2A (01_Schema_Y_StoredProcedures_Fase2A.sql).
--
-- BLOQUE 2 (ver 08_Migracion_ProgramacionMensual.sql y
-- 09_ProgramacionMensual.sql): sp_TI_DecisionesPrioridad_Crear y
-- sp_TI_AprobacionesPrioridad_Registrar en este archivo ya NO dependen de
-- una evaluacion de 6 criterios (TI_EvaluacionesPrioridad/_Detalle quedan
-- sin uso en el flujo nuevo, pero su historico y sus SP NO se eliminan).
--
-- REQUIERE que 07_Migracion_DiasEstimados.sql ya se haya ejecutado (agrega
-- dbo.TI_Funcionalidades.DiasEstimados, NULLABLE). La estimacion de alcance
-- se captura en dias por funcionalidad, dentro de la misma definicion de
-- alcance -- no existe ya un total capturado manualmente, se deriva
-- siempre con SUM() (ver sp_TI_VersionesAlcance_Obtener).
--
-- dbo.sp_TI_Solicitudes_CambiarEstado (Fase 2A) NO SE MODIFICA EN SU CUERPO.
-- Unicamente se amplia su catalogo interno de transiciones permitidas
-- (bloque "Catalogo de transiciones habilitadas") para los nuevos codigos
-- de esta fase. El resto del procedimiento permanece exactamente igual.
--
-- Decisiones de negocio y de diseno ya aprobadas en el cierre de la
-- Fase 2B-0 (resumen, ver el historial de la conversacion para el detalle
-- completo de cada regla):
--   - Al abrir una version de alcance nueva, la solicitud regresa a
--     DEFINICION_ALCANCE (salvo que ya este ahi), via EXEC al mecanismo
--     central.
--   - La version previamente APROBADA conserva ese estado mientras la
--     nueva este BORRADOR/PENDIENTE_APROBACION; solo se sustituye
--     (-> SUSTITUIDA) cuando la nueva recibe las 2 aprobaciones, en la
--     misma transaccion que la pone en APROBADA.
--   - Vigencia operativa de una version APROBADA: deja de admitir nuevas
--     estimaciones/evaluaciones en cuanto existe una sucesora en
--     BORRADOR/PENDIENTE_APROBACION, aunque su EstadoVersion siga diciendo
--     APROBADA (conservacion historica vs. vigencia operativa).
--   - No se puede abrir una version de alcance nueva mientras exista una
--     decision de prioridad PENDIENTE para la misma solicitud (evita
--     flujos paralelos incompatibles).
--   - Formula de prioridad: SUM(CAST(Puntuacion AS DECIMAL(5,2))/5*PesoAplicado),
--     aritmetica decimal exacta.
--   - Las 6 puntuaciones de una evaluacion de prioridad son obligatorias
--     desde su creacion (no se admite una evaluacion con criterios sin
--     responder); los 6 pesos se congelan de un catalogo consistente bajo
--     HOLDLOCK en el mismo INSERT; PesoAplicado nunca se reescribe;
--     Puntuacion/Comentario solo editables mientras EstadoEvaluacion='BORRADOR'.
--   - Horas generales de una estimacion se DERIVAN de la suma del detalle
--     por funcionalidad -- no se reciben como parametro independiente.
--   - TipoDecision (que decidio el negocio) y EstadoDecision (si el flujo
--     de aprobacion llego a termino) son ejes independientes.
--   - TipoAprobador se valida dentro del propio SP contra la relacion real
--     del usuario (SolicitanteUsuarioId / rol JEFATURA_TI / rol
--     GERENCIA_PLANEACION) -- nunca se confia en un valor libre del cliente.
--   - Generacion de NumeroVersion/NumeroEvaluacion/NumeroDecision protegida
--     con sp_getapplock (@LockOwner='Transaction') por SolicitudId, con
--     @LockResult validado antes de cualquier MAX()+1.
--   - Aprobaciones duales (alcance: SOLICITANTE+JEFATURA_TI; prioridad:
--     JEFATURA_TI+GERENCIA_PLANEACION) serializadas con sp_getapplock por
--     VersionAlcanceId/DecisionPrioridadId; re-chequeo del estado DESPUES
--     de adquirir el bloqueo; una aprobacion ya resuelta (<> PENDIENTE)
--     nunca se sobrescribe; misma persona no puede cubrir ambos lados.
--   - SET XACT_ABORT ON + BEGIN TRY/TRANSACTION...COMMIT + CATCH con
--     IF XACT_STATE()<>0 ROLLBACK + THROW, en todos los SP de esta fase.
--   - Ningun parametro ni variable de estado se compara con NOT IN/<> sin
--     antes descartar explicitamente el caso NULL (NULL NOT IN(...) y
--     NULL<>'x' evaluan a UNKNOWN en T-SQL, que un IF trata como FALSE).
--
-- SUPUESTO AUN NO CONFIRMADO POR EL RESPONSABLE (documentado en Fase 2B-0,
-- seccion 2 del comentario del script de creacion del esquema): la formula
-- de puntaje aqui implementada es Puntuacion/5*PesoAplicado, conforme a la
-- especificacion funcional aprobada para esta fase. Si el script de
-- creacion de tablas (fuera de este repositorio) realmente define
-- (Puntuacion-1)/4.0*PesoAplicado como pretende reflejar el diseno
-- original, debe confirmarse con el responsable antes de usarse en
-- produccion -- no se modifica aqui sin esa confirmacion explicita.
-- ============================================================================

-- ----------------------------------------------------------------------------
-- VERSIONES DE ALCANCE Y FUNCIONALIDADES
-- ----------------------------------------------------------------------------

CREATE OR ALTER PROCEDURE dbo.sp_TI_VersionesAlcance_Crear
    @SolicitudId BIGINT,
    @Objetivo NVARCHAR(MAX),
    @AlcanceIncluido NVARCHAR(MAX),
    @AlcanceExcluido NVARCHAR(MAX) = NULL,
    @Supuestos NVARCHAR(MAX) = NULL,
    @Dependencias NVARCHAR(MAX) = NULL,
    @usuarioId INT,
    @json NVARCHAR(MAX)   -- array: [{ Codigo, Nombre, Descripcion, ReglasNegocio, CriteriosAceptacion, PrioridadInterna, Complejidad, Orden, DiasEstimados }]
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @SolicitudId, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        DECLARE @EstadoActualCodigo NVARCHAR(100) = (
            SELECT es.Codigo FROM dbo.TI_Solicitudes s
            INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
            WHERE s.SolicitudId = @SolicitudId
        );
        IF (@EstadoActualCodigo IS NULL)
        BEGIN
            RAISERROR('Solicitud no encontrada', 16, 1);
            RETURN;
        END
        IF (@EstadoActualCodigo NOT IN ('EN_REVISION', 'DEFINICION_ALCANCE', 'EVALUACION_TECNICA', 'PENDIENTE_PRIORIZACION', 'PRIORIZADA', 'EN_BACKLOG'))
        BEGIN
            RAISERROR('La solicitud no admite una nueva version de alcance en su estado actual', 16, 1);
            RETURN;
        END
        IF EXISTS (SELECT 1 FROM dbo.TI_VersionesAlcance WHERE SolicitudId = @SolicitudId AND EstadoVersion IN ('BORRADOR', 'PENDIENTE_APROBACION'))
        BEGIN
            RAISERROR('Ya existe una version de alcance en curso para esta solicitud', 16, 1);
            RETURN;
        END
        IF EXISTS (SELECT 1 FROM dbo.TI_DecisionesPrioridad WHERE SolicitudId = @SolicitudId AND EstadoDecision = 'PENDIENTE')
        BEGIN
            RAISERROR('Existe una decision de prioridad pendiente de aprobacion; no se puede abrir una nueva version de alcance hasta resolverla', 16, 1);
            RETURN;
        END

        DECLARE @VersionAnteriorId BIGINT = (SELECT VersionAlcanceId FROM dbo.TI_VersionesAlcance WHERE SolicitudId = @SolicitudId AND EstadoVersion = 'APROBADA');
        DECLARE @NumeroVersionNueva INT = (SELECT ISNULL(MAX(NumeroVersion), 0) + 1 FROM dbo.TI_VersionesAlcance WHERE SolicitudId = @SolicitudId);

        INSERT INTO dbo.TI_VersionesAlcance (SolicitudId, NumeroVersion, VersionAnteriorId, Objetivo, AlcanceIncluido, AlcanceExcluido, Supuestos, Dependencias, EstadoVersion, CreadaPorUsuarioId, FechaCreacion)
        VALUES (@SolicitudId, @NumeroVersionNueva, @VersionAnteriorId, @Objetivo, @AlcanceIncluido, @AlcanceExcluido, @Supuestos, @Dependencias, 'BORRADOR', @usuarioId, SYSUTCDATETIME());

        DECLARE @VersionAlcanceId BIGINT = SCOPE_IDENTITY();

        INSERT INTO dbo.TI_Funcionalidades (VersionAlcanceId, Codigo, Nombre, Descripcion, ReglasNegocio, CriteriosAceptacion, PrioridadInterna, Complejidad, Orden, DiasEstimados, Activa, CreadaPorUsuarioId, FechaCreacion)
        SELECT @VersionAlcanceId, Codigo, Nombre, Descripcion, ReglasNegocio, CriteriosAceptacion, PrioridadInterna, Complejidad, Orden, DiasEstimados, 1, @usuarioId, SYSUTCDATETIME()
        FROM OPENJSON(@json) WITH (
            Codigo NVARCHAR(30), Nombre NVARCHAR(200), Descripcion NVARCHAR(MAX),
            ReglasNegocio NVARCHAR(MAX), CriteriosAceptacion NVARCHAR(MAX),
            PrioridadInterna TINYINT, Complejidad NVARCHAR(20), Orden INT,
            DiasEstimados DECIMAL(6,2)
        );

        IF (@EstadoActualCodigo <> 'DEFINICION_ALCANCE')
        BEGIN
            EXEC dbo.sp_TI_Solicitudes_CambiarEstado
                @SolicitudId = @SolicitudId,
                @EstadoNuevoCodigo = 'DEFINICION_ALCANCE',
                @usuarioId = @usuarioId,
                @Motivo = NULL;
        END

        COMMIT TRANSACTION;
        SELECT @VersionAlcanceId AS VersionAlcanceId, @NumeroVersionNueva AS NumeroVersion;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_VersionesAlcance_Obtener
    @VersionAlcanceId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    -- TotalDiasEstimados: SUM() sobre las funcionalidades activas, nunca un
    -- total capturado manualmente. SUM() de columnas NULL ignora los NULL;
    -- si NINGUNA funcionalidad activa tiene DiasEstimados capturado, el
    -- resultado es NULL (no 0), para distinguir "sin estimar" de "cero".
    SELECT v.VersionAlcanceId, v.SolicitudId, v.NumeroVersion, v.VersionAnteriorId, v.Objetivo, v.AlcanceIncluido, v.AlcanceExcluido,
           v.Supuestos, v.Dependencias, v.EstadoVersion, v.CreadaPorUsuarioId, v.FechaCreacion, v.FechaEnvioAprobacion, v.FechaResolucion, v.ObservacionesResolucion,
           (SELECT SUM(f.DiasEstimados) FROM dbo.TI_Funcionalidades f WHERE f.VersionAlcanceId = v.VersionAlcanceId AND f.Activa = 1) AS TotalDiasEstimados
    FROM dbo.TI_VersionesAlcance v
    WHERE v.VersionAlcanceId = @VersionAlcanceId;

    SELECT FuncionalidadId, VersionAlcanceId, Codigo, Nombre, Descripcion, ReglasNegocio, CriteriosAceptacion, PrioridadInterna, Complejidad, Orden, DiasEstimados, Activa, CreadaPorUsuarioId, FechaCreacion
    FROM dbo.TI_Funcionalidades
    WHERE VersionAlcanceId = @VersionAlcanceId AND Activa = 1
    ORDER BY Orden;
END
GO

-- No existia ninguna operacion para modificar DiasEstimados despues de
-- creada la version (sp_TI_VersionesAlcance_Crear solo lo recibe en el
-- alta) -- este es el cambio minimo para permitirlo. Solo se permite
-- mientras la version de la funcionalidad este en BORRADOR; enviada a
-- aprobacion, aprobada o sustituida quedan fuera, igual que ya protege
-- sp_TI_VersionesAlcance_EnviarAprobacion con su propia validacion.
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_ActualizarDiasEstimados
    @FuncionalidadId BIGINT,
    @DiasEstimados DECIMAL(6, 2) = NULL,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@DiasEstimados IS NOT NULL AND @DiasEstimados < 0)
        BEGIN
            RAISERROR('Los dias estimados no pueden ser negativos', 16, 1);
            RETURN;
        END

        DECLARE @EstadoVersionActual NVARCHAR(30);
        SELECT @EstadoVersionActual = v.EstadoVersion
        FROM dbo.TI_Funcionalidades f
        INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
        WHERE f.FuncionalidadId = @FuncionalidadId AND f.Activa = 1;

        IF (@EstadoVersionActual IS NULL)
        BEGIN
            RAISERROR('La funcionalidad indicada no existe o no esta activa', 16, 1);
            RETURN;
        END
        IF (@EstadoVersionActual <> 'BORRADOR')
        BEGIN
            RAISERROR('Solo se pueden editar los dias estimados mientras la version esta en BORRADOR', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_Funcionalidades SET DiasEstimados = @DiasEstimados WHERE FuncionalidadId = @FuncionalidadId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_VersionesAlcance_ListadoPorSolicitud
    @SolicitudId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT VersionAlcanceId, SolicitudId, NumeroVersion, VersionAnteriorId, Objetivo, EstadoVersion, CreadaPorUsuarioId, FechaCreacion, FechaEnvioAprobacion, FechaResolucion
    FROM dbo.TI_VersionesAlcance
    WHERE SolicitudId = @SolicitudId
    ORDER BY NumeroVersion DESC;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_VersionesAlcance_EnviarAprobacion
    @VersionAlcanceId BIGINT,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @SolicitudId BIGINT, @EstadoVersionActual NVARCHAR(30);
        SELECT @SolicitudId = SolicitudId, @EstadoVersionActual = EstadoVersion
        FROM dbo.TI_VersionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La version de alcance indicada no existe', 16, 1);
            RETURN;
        END
        IF (@EstadoVersionActual <> 'BORRADOR')
        BEGIN
            RAISERROR('Solo una version en BORRADOR puede enviarse a aprobacion', 16, 1);
            RETURN;
        END
        IF NOT EXISTS (SELECT 1 FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId = @VersionAlcanceId AND Activa = 1)
        BEGIN
            RAISERROR('La version no tiene funcionalidades activas', 16, 1);
            RETURN;
        END
        IF EXISTS (SELECT 1 FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId = @VersionAlcanceId AND Activa = 1 AND (DiasEstimados IS NULL OR DiasEstimados < 0))
        BEGIN
            RAISERROR('Todas las funcionalidades activas deben tener una estimacion en dias valida (no negativa) antes de enviar a aprobacion', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_VersionesAlcance SET EstadoVersion = 'PENDIENTE_APROBACION', FechaEnvioAprobacion = SYSUTCDATETIME() WHERE VersionAlcanceId = @VersionAlcanceId;

        INSERT INTO dbo.TI_AprobacionesAlcance (VersionAlcanceId, TipoAprobador, EstadoAprobacion, FechaSolicitud)
        VALUES (@VersionAlcanceId, 'SOLICITANTE', 'PENDIENTE', SYSUTCDATETIME()),
               (@VersionAlcanceId, 'JEFATURA_TI', 'PENDIENTE', SYSUTCDATETIME());

        EXEC dbo.sp_TI_Solicitudes_CambiarEstado
            @SolicitudId = @SolicitudId,
            @EstadoNuevoCodigo = 'PENDIENTE_AUTORIZACION_ALCANCE',
            @usuarioId = @usuarioId,
            @Motivo = NULL;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- TipoAprobador se valida contra la relacion real del usuario con la
-- version (SolicitanteUsuarioId de la solicitud, o rol JEFATURA_TI activo)
-- -- nunca se confia en el valor que mande el cliente.
CREATE OR ALTER PROCEDURE dbo.sp_TI_AprobacionesAlcance_Registrar
    @VersionAlcanceId BIGINT,
    @TipoAprobador NVARCHAR(30),
    @EstadoDecision NVARCHAR(20),
    @Comentario NVARCHAR(2000) = NULL,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@TipoAprobador IS NULL OR @EstadoDecision IS NULL)
        BEGIN
            RAISERROR('Los parametros de la aprobacion son obligatorios', 16, 1);
            RETURN;
        END
        IF (@TipoAprobador NOT IN ('SOLICITANTE', 'JEFATURA_TI'))
        BEGIN
            RAISERROR('Tipo de aprobador invalido', 16, 1);
            RETURN;
        END
        IF (@EstadoDecision NOT IN ('APROBADA', 'RECHAZADA'))
        BEGIN
            RAISERROR('El estado de aprobacion solicitado es invalido', 16, 1);
            RETURN;
        END

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @VersionAlcanceId, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        DECLARE @SolicitudId BIGINT, @EstadoVersionActual NVARCHAR(30);
        SELECT @SolicitudId = SolicitudId, @EstadoVersionActual = EstadoVersion
        FROM dbo.TI_VersionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La version de alcance indicada no existe', 16, 1);
            RETURN;
        END
        IF (@EstadoVersionActual <> 'PENDIENTE_APROBACION')
        BEGIN
            RAISERROR('La version ya no admite aprobaciones', 16, 1);
            RETURN;
        END

        -- Autorizacion efectiva: el usuario debe tener realmente la relacion
        -- que el TipoAprobador dice representar.
        IF (@TipoAprobador = 'SOLICITANTE')
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM dbo.TI_Solicitudes WHERE SolicitudId = @SolicitudId AND SolicitanteUsuarioId = @usuarioId)
            BEGIN
                RAISERROR('El usuario no es el solicitante de esta version de alcance', 16, 1);
                RETURN;
            END
        END
        ELSE -- JEFATURA_TI
        BEGIN
            IF NOT EXISTS (
                SELECT 1 FROM dbo.TI_UsuarioRoles ur INNER JOIN dbo.TI_Roles r ON r.RolId = ur.RolId
                WHERE ur.UsuarioId = @usuarioId AND ur.Activo = 1 AND r.Codigo = 'JEFATURA_TI'
            )
            BEGIN
                RAISERROR('El usuario no tiene el rol JEFATURA_TI', 16, 1);
                RETURN;
            END
        END

        DECLARE @EstadoAprobacionActual NVARCHAR(20);
        SELECT @EstadoAprobacionActual = EstadoAprobacion
        FROM dbo.TI_AprobacionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId AND TipoAprobador = @TipoAprobador;

        IF (@EstadoAprobacionActual IS NULL)
        BEGIN
            RAISERROR('No existe la fila de aprobacion para el tipo de aprobador indicado', 16, 1);
            RETURN;
        END
        IF (@EstadoAprobacionActual <> 'PENDIENTE')
        BEGIN
            RAISERROR('Esta aprobacion ya fue resuelta anteriormente y no puede modificarse', 16, 1);
            RETURN;
        END

        DECLARE @AprobadorLadoContrario INT = (
            SELECT AprobadorUsuarioId FROM dbo.TI_AprobacionesAlcance
            WHERE VersionAlcanceId = @VersionAlcanceId AND TipoAprobador <> @TipoAprobador
        );
        IF (@usuarioId = @AprobadorLadoContrario)
        BEGIN
            RAISERROR('La misma persona no puede autorizar ambos lados', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_AprobacionesAlcance
           SET EstadoAprobacion = @EstadoDecision, FechaDecision = SYSUTCDATETIME(), Comentario = @Comentario, AprobadorUsuarioId = @usuarioId
         WHERE VersionAlcanceId = @VersionAlcanceId AND TipoAprobador = @TipoAprobador;

        IF (@EstadoDecision = 'RECHAZADA')
        BEGIN
            UPDATE dbo.TI_VersionesAlcance SET EstadoVersion = 'RECHAZADA', FechaResolucion = SYSUTCDATETIME(), ObservacionesResolucion = @Comentario WHERE VersionAlcanceId = @VersionAlcanceId;
            -- la fila hermana PENDIENTE no se toca; la solicitud ya esta en
            -- DEFINICION_ALCANCE desde que se abrio esta version, no hace
            -- falta ninguna transicion adicional (seria un no-op).
        END
        ELSE
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM dbo.TI_AprobacionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId AND EstadoAprobacion <> 'APROBADA')
            BEGIN
                DECLARE @VersionAnteriorId BIGINT = (SELECT VersionAnteriorId FROM dbo.TI_VersionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId);

                IF (@VersionAnteriorId IS NOT NULL)
                    UPDATE dbo.TI_VersionesAlcance SET EstadoVersion = 'SUSTITUIDA' WHERE VersionAlcanceId = @VersionAnteriorId;

                UPDATE dbo.TI_VersionesAlcance SET EstadoVersion = 'APROBADA', FechaResolucion = SYSUTCDATETIME() WHERE VersionAlcanceId = @VersionAlcanceId;

                -- Bloque 2: sp_TI_VersionesAlcance_EnviarAprobacion ya exige
                -- que todas las funcionalidades activas tengan DiasEstimados
                -- validos antes de poder enviarse a aprobacion, asi que al
                -- completarse la aprobacion dual del alcance ya no hace falta
                -- pasar por EVALUACION_TECNICA (paso que antes resolvia la
                -- pantalla de estimacion independiente, retirada en el
                -- Bloque 1): se avanza directo a PENDIENTE_PRIORIZACION.
                EXEC dbo.sp_TI_Solicitudes_CambiarEstado
                    @SolicitudId = @SolicitudId, @EstadoNuevoCodigo = 'PENDIENTE_PRIORIZACION', @usuarioId = @usuarioId, @Motivo = NULL;
            END
            -- si queda 1 APROBADA y la otra todavia PENDIENTE: no se hace nada mas
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- ESTIMACIONES
-- ----------------------------------------------------------------------------

-- Horas generales DERIVADAS de la suma del detalle por funcionalidad -- no
-- se reciben como parametro independiente, para eliminar discrepancias.
CREATE OR ALTER PROCEDURE dbo.sp_TI_Estimaciones_Crear
    @SolicitudId BIGINT,
    @VersionAlcanceId BIGINT,
    @RegistradaPorUsuarioId INT,
    @Riesgos NVARCHAR(MAX) = NULL,
    @Supuestos NVARCHAR(MAX) = NULL,
    @Dependencias NVARCHAR(MAX) = NULL,
    @DiasCalendarioEstimados INT = NULL,
    @json NVARCHAR(MAX)   -- array: [{ FuncionalidadId, HorasAnalisis, HorasDesarrollo, HorasPruebas, HorasDespliegue, Observaciones }]
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @SolicitudId, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.TI_VersionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId AND SolicitudId = @SolicitudId AND EstadoVersion = 'APROBADA')
        BEGIN
            RAISERROR('La version de alcance no esta aprobada o no pertenece a la solicitud indicada', 16, 1);
            RETURN;
        END
        IF EXISTS (SELECT 1 FROM dbo.TI_VersionesAlcance WHERE VersionAnteriorId = @VersionAlcanceId AND EstadoVersion IN ('BORRADOR', 'PENDIENTE_APROBACION'))
        BEGIN
            RAISERROR('Existe una version de alcance posterior en tramite', 16, 1);
            RETURN;
        END

        DECLARE @EstadoActualCodigo NVARCHAR(100) = (
            SELECT es.Codigo FROM dbo.TI_Solicitudes s
            INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
            WHERE s.SolicitudId = @SolicitudId
        );
        IF (@EstadoActualCodigo IS NULL)
        BEGIN
            RAISERROR('Solicitud no encontrada', 16, 1);
            RETURN;
        END
        IF (@EstadoActualCodigo <> 'EVALUACION_TECNICA')
        BEGIN
            RAISERROR('La solicitud no se encuentra en fase de evaluacion tecnica', 16, 1);
            RETURN;
        END

        DECLARE @FuncionalidadesVersion TABLE (FuncionalidadId BIGINT PRIMARY KEY);
        INSERT INTO @FuncionalidadesVersion
        SELECT FuncionalidadId FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId = @VersionAlcanceId AND Activa = 1;

        IF (SELECT COUNT(*) FROM @FuncionalidadesVersion) = 0
        BEGIN
            RAISERROR('La version de alcance no tiene funcionalidades activas para estimar', 16, 1);
            RETURN;
        END

        DECLARE @DetalleJson TABLE (FuncionalidadId BIGINT, HorasAnalisis DECIMAL(10,2), HorasDesarrollo DECIMAL(10,2), HorasPruebas DECIMAL(10,2), HorasDespliegue DECIMAL(10,2), Observaciones NVARCHAR(1000));
        INSERT INTO @DetalleJson
        SELECT FuncionalidadId, HorasAnalisis, HorasDesarrollo, HorasPruebas, HorasDespliegue, Observaciones
        FROM OPENJSON(@json) WITH (FuncionalidadId BIGINT, HorasAnalisis DECIMAL(10,2), HorasDesarrollo DECIMAL(10,2), HorasPruebas DECIMAL(10,2), HorasDespliegue DECIMAL(10,2), Observaciones NVARCHAR(1000));

        IF (SELECT COUNT(*) FROM @DetalleJson) <> (SELECT COUNT(DISTINCT FuncionalidadId) FROM @DetalleJson)
        BEGIN
            RAISERROR('El detalle de estimacion contiene funcionalidades duplicadas', 16, 1);
            RETURN;
        END
        IF EXISTS (SELECT 1 FROM @DetalleJson d LEFT JOIN @FuncionalidadesVersion f ON f.FuncionalidadId = d.FuncionalidadId WHERE f.FuncionalidadId IS NULL)
        BEGIN
            RAISERROR('El detalle incluye funcionalidades que no pertenecen a la version de alcance indicada', 16, 1);
            RETURN;
        END
        IF EXISTS (SELECT 1 FROM @FuncionalidadesVersion f LEFT JOIN @DetalleJson d ON d.FuncionalidadId = f.FuncionalidadId WHERE d.FuncionalidadId IS NULL)
        BEGIN
            RAISERROR('Falta el detalle de estimacion de alguna funcionalidad activa de la version', 16, 1);
            RETURN;
        END
        IF EXISTS (
            SELECT 1 FROM @DetalleJson
            WHERE HorasAnalisis IS NULL OR HorasDesarrollo IS NULL OR HorasPruebas IS NULL OR HorasDespliegue IS NULL
               OR HorasAnalisis < 0 OR HorasDesarrollo < 0 OR HorasPruebas < 0 OR HorasDespliegue < 0
        )
        BEGIN
            RAISERROR('Cada funcionalidad debe incluir horas numericas validas y no negativas en los 4 rubros', 16, 1);
            RETURN;
        END

        DECLARE @HorasAnalisis DECIMAL(10,2) = (SELECT SUM(HorasAnalisis) FROM @DetalleJson);
        DECLARE @HorasDesarrollo DECIMAL(10,2) = (SELECT SUM(HorasDesarrollo) FROM @DetalleJson);
        DECLARE @HorasPruebas DECIMAL(10,2) = (SELECT SUM(HorasPruebas) FROM @DetalleJson);
        DECLARE @HorasDespliegue DECIMAL(10,2) = (SELECT SUM(HorasDespliegue) FROM @DetalleJson);

        DECLARE @NumeroVersionNueva INT = (SELECT ISNULL(MAX(NumeroVersion), 0) + 1 FROM dbo.TI_Estimaciones WHERE SolicitudId = @SolicitudId);

        INSERT INTO dbo.TI_Estimaciones (SolicitudId, VersionAlcanceId, NumeroVersion, EstadoEstimacion, HorasAnalisis, HorasDesarrollo, HorasPruebas, HorasDespliegue, Riesgos, Supuestos, Dependencias, DiasCalendarioEstimados, RegistradaPorUsuarioId, FechaEstimacion)
        VALUES (@SolicitudId, @VersionAlcanceId, @NumeroVersionNueva, 'BORRADOR', @HorasAnalisis, @HorasDesarrollo, @HorasPruebas, @HorasDespliegue, @Riesgos, @Supuestos, @Dependencias, @DiasCalendarioEstimados, @RegistradaPorUsuarioId, SYSUTCDATETIME());
        -- HorasTotal: columna calculada y persistida, nunca se incluye aqui.

        DECLARE @EstimacionId BIGINT = SCOPE_IDENTITY();

        INSERT INTO dbo.TI_EstimacionDetalle (EstimacionId, FuncionalidadId, HorasAnalisis, HorasDesarrollo, HorasPruebas, HorasDespliegue, Observaciones)
        SELECT @EstimacionId, FuncionalidadId, HorasAnalisis, HorasDesarrollo, HorasPruebas, HorasDespliegue, Observaciones FROM @DetalleJson;

        COMMIT TRANSACTION;
        SELECT @EstimacionId AS EstimacionId, @NumeroVersionNueva AS NumeroVersion;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_Estimaciones_Obtener
    @EstimacionId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT EstimacionId, SolicitudId, VersionAlcanceId, NumeroVersion, EstadoEstimacion, HorasAnalisis, HorasDesarrollo, HorasPruebas, HorasDespliegue, HorasTotal,
           Riesgos, Supuestos, Dependencias, DiasCalendarioEstimados, RegistradaPorUsuarioId, FechaEstimacion, FechaValidacion
    FROM dbo.TI_Estimaciones
    WHERE EstimacionId = @EstimacionId;

    SELECT EstimacionDetalleId, EstimacionId, FuncionalidadId, HorasAnalisis, HorasDesarrollo, HorasPruebas, HorasDespliegue, Observaciones
    FROM dbo.TI_EstimacionDetalle
    WHERE EstimacionId = @EstimacionId;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_Estimaciones_Validar
    @EstimacionId BIGINT,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @SolicitudId BIGINT, @EstadoEstimacionActual NVARCHAR(20);
        SELECT @SolicitudId = SolicitudId, @EstadoEstimacionActual = EstadoEstimacion
        FROM dbo.TI_Estimaciones WHERE EstimacionId = @EstimacionId;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La estimacion indicada no existe', 16, 1);
            RETURN;
        END
        IF (@EstadoEstimacionActual <> 'BORRADOR')
        BEGIN
            RAISERROR('La estimacion ya fue validada o sustituida anteriormente', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_Estimaciones SET EstadoEstimacion = 'VALIDADA', FechaValidacion = SYSUTCDATETIME() WHERE EstimacionId = @EstimacionId;

        EXEC dbo.sp_TI_Solicitudes_CambiarEstado
            @SolicitudId = @SolicitudId, @EstadoNuevoCodigo = 'PENDIENTE_PRIORIZACION', @usuarioId = @usuarioId, @Motivo = NULL;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- PRIORIZACION
-- ----------------------------------------------------------------------------

-- Las 6 puntuaciones son obligatorias desde la creacion; los 6 pesos se
-- congelan de una sola lectura bloqueada del catalogo (HOLDLOCK) para que
-- nunca queden mezclados entre dos configuraciones distintas.
CREATE OR ALTER PROCEDURE dbo.sp_TI_EvaluacionesPrioridad_Crear
    @SolicitudId BIGINT,
    @VersionAlcanceId BIGINT,
    @EvaluadaPorUsuarioId INT,
    @json NVARCHAR(MAX)   -- array: [{ CriterioPrioridadId, Puntuacion, Comentario }] x 6
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @SolicitudId, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.TI_VersionesAlcance WHERE VersionAlcanceId = @VersionAlcanceId AND SolicitudId = @SolicitudId AND EstadoVersion = 'APROBADA')
        BEGIN
            RAISERROR('La version de alcance no esta aprobada o no pertenece a la solicitud indicada', 16, 1);
            RETURN;
        END
        IF EXISTS (SELECT 1 FROM dbo.TI_VersionesAlcance WHERE VersionAnteriorId = @VersionAlcanceId AND EstadoVersion IN ('BORRADOR', 'PENDIENTE_APROBACION'))
        BEGIN
            RAISERROR('Existe una version de alcance posterior en tramite', 16, 1);
            RETURN;
        END
        IF NOT EXISTS (SELECT 1 FROM dbo.TI_Estimaciones WHERE VersionAlcanceId = @VersionAlcanceId AND SolicitudId = @SolicitudId AND EstadoEstimacion = 'VALIDADA')
        BEGIN
            RAISERROR('No existe una estimacion validada para esta version de alcance y solicitud', 16, 1);
            RETURN;
        END

        DECLARE @EstadoActualCodigo NVARCHAR(100) = (
            SELECT es.Codigo FROM dbo.TI_Solicitudes s
            INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
            WHERE s.SolicitudId = @SolicitudId
        );
        IF (@EstadoActualCodigo IS NULL)
        BEGIN
            RAISERROR('Solicitud no encontrada', 16, 1);
            RETURN;
        END
        IF (@EstadoActualCodigo NOT IN ('PENDIENTE_PRIORIZACION', 'PRIORIZADA', 'EN_BACKLOG'))
        BEGIN
            RAISERROR('La solicitud no se encuentra en una fase que admita evaluacion de prioridad', 16, 1);
            RETURN;
        END

        DECLARE @Criterios TABLE (CriterioPrioridadId INT PRIMARY KEY, Peso DECIMAL(5,2));
        INSERT INTO @Criterios SELECT CriterioPrioridadId, Peso FROM dbo.TI_CriteriosPrioridad WITH (HOLDLOCK, ROWLOCK) WHERE Activo = 1;
        IF (SELECT COUNT(*) FROM @Criterios) <> 6 OR (SELECT SUM(Peso) FROM @Criterios) <> 100
        BEGIN
            RAISERROR('El catalogo de criterios activos no tiene 6 elementos con pesos que sumen 100', 16, 1);
            RETURN;
        END

        DECLARE @Detalle TABLE (CriterioPrioridadId INT, Puntuacion TINYINT, Comentario NVARCHAR(1000));
        INSERT INTO @Detalle SELECT CriterioPrioridadId, Puntuacion, Comentario FROM OPENJSON(@json) WITH (CriterioPrioridadId INT, Puntuacion TINYINT, Comentario NVARCHAR(1000));

        IF EXISTS (SELECT 1 FROM @Detalle WHERE Puntuacion IS NULL)
        BEGIN RAISERROR('Ninguna puntuacion puede quedar sin responder', 16, 1); RETURN; END
        IF EXISTS (SELECT 1 FROM @Detalle WHERE Puntuacion NOT BETWEEN 1 AND 5)
        BEGIN RAISERROR('Cada puntuacion debe estar entre 1 y 5', 16, 1); RETURN; END
        IF (SELECT COUNT(*) FROM @Detalle) <> 6 OR (SELECT COUNT(DISTINCT CriterioPrioridadId) FROM @Detalle) <> 6
        BEGIN RAISERROR('Debe enviar exactamente una puntuacion por cada uno de los 6 criterios activos, sin duplicados', 16, 1); RETURN; END
        IF EXISTS (SELECT 1 FROM @Detalle d LEFT JOIN @Criterios c ON c.CriterioPrioridadId = d.CriterioPrioridadId WHERE c.CriterioPrioridadId IS NULL)
           OR EXISTS (SELECT 1 FROM @Criterios c LEFT JOIN @Detalle d ON d.CriterioPrioridadId = c.CriterioPrioridadId WHERE d.CriterioPrioridadId IS NULL)
        BEGIN RAISERROR('Los criterios enviados no coinciden exactamente con los 6 criterios activos del catalogo', 16, 1); RETURN; END

        DECLARE @NumeroEvaluacionNueva INT = (SELECT ISNULL(MAX(NumeroEvaluacion), 0) + 1 FROM dbo.TI_EvaluacionesPrioridad WHERE SolicitudId = @SolicitudId);

        INSERT INTO dbo.TI_EvaluacionesPrioridad (SolicitudId, VersionAlcanceId, NumeroEvaluacion, EstadoEvaluacion, EvaluadaPorUsuarioId, FechaCreacion)
        VALUES (@SolicitudId, @VersionAlcanceId, @NumeroEvaluacionNueva, 'BORRADOR', @EvaluadaPorUsuarioId, SYSUTCDATETIME());

        DECLARE @EvaluacionPrioridadId BIGINT = SCOPE_IDENTITY();

        INSERT INTO dbo.TI_EvaluacionPrioridadDetalle (EvaluacionPrioridadId, CriterioPrioridadId, Puntuacion, PesoAplicado, Comentario)
        SELECT @EvaluacionPrioridadId, d.CriterioPrioridadId, d.Puntuacion, c.Peso, d.Comentario
        FROM @Detalle d INNER JOIN @Criterios c ON c.CriterioPrioridadId = d.CriterioPrioridadId;

        COMMIT TRANSACTION;
        SELECT @EvaluacionPrioridadId AS EvaluacionPrioridadId, @NumeroEvaluacionNueva AS NumeroEvaluacion;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Solo Puntuacion/Comentario, solo mientras EstadoEvaluacion='BORRADOR';
-- PesoAplicado nunca se reescribe aqui.
CREATE OR ALTER PROCEDURE dbo.sp_TI_EvaluacionesPrioridad_RegistrarDetalle
    @EvaluacionPrioridadId BIGINT,
    @CriterioPrioridadId INT,
    @Puntuacion TINYINT,
    @Comentario NVARCHAR(1000) = NULL,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@Puntuacion IS NULL)
        BEGIN
            RAISERROR('La puntuacion es obligatoria', 16, 1);
            RETURN;
        END
        IF (@Puntuacion NOT BETWEEN 1 AND 5)
        BEGIN
            RAISERROR('La puntuacion debe estar entre 1 y 5', 16, 1);
            RETURN;
        END

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @EvaluacionPrioridadId, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        DECLARE @EstadoEvaluacionActual NVARCHAR(20);
        SELECT @EstadoEvaluacionActual = EstadoEvaluacion FROM dbo.TI_EvaluacionesPrioridad WHERE EvaluacionPrioridadId = @EvaluacionPrioridadId;

        IF (@EstadoEvaluacionActual IS NULL)
        BEGIN
            RAISERROR('La evaluacion no existe', 16, 1);
            RETURN;
        END
        IF (@EstadoEvaluacionActual <> 'BORRADOR')
        BEGIN
            RAISERROR('La evaluacion ya fue completada y no admite modificaciones', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.TI_EvaluacionPrioridadDetalle WHERE EvaluacionPrioridadId = @EvaluacionPrioridadId AND CriterioPrioridadId = @CriterioPrioridadId)
        BEGIN
            RAISERROR('No existe el detalle para ese criterio en esta evaluacion', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_EvaluacionPrioridadDetalle
           SET Puntuacion = @Puntuacion, Comentario = @Comentario
         WHERE EvaluacionPrioridadId = @EvaluacionPrioridadId AND CriterioPrioridadId = @CriterioPrioridadId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Formula aprobada para esta fase: SUM(Puntuacion/5*PesoAplicado), aritmetica
-- decimal exacta (ver nota de discrepancia documentada al inicio del script).
CREATE OR ALTER PROCEDURE dbo.sp_TI_EvaluacionesPrioridad_Completar
    @EvaluacionPrioridadId BIGINT,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @EvaluacionPrioridadId, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        DECLARE @EstadoEvaluacionActual NVARCHAR(20);
        SELECT @EstadoEvaluacionActual = EstadoEvaluacion FROM dbo.TI_EvaluacionesPrioridad WHERE EvaluacionPrioridadId = @EvaluacionPrioridadId;

        IF (@EstadoEvaluacionActual IS NULL)
        BEGIN
            RAISERROR('La evaluacion no existe', 16, 1);
            RETURN;
        END
        IF (@EstadoEvaluacionActual <> 'BORRADOR')
        BEGIN
            RAISERROR('La evaluacion ya fue completada anteriormente', 16, 1);
            RETURN;
        END

        IF (SELECT COUNT(*) FROM dbo.TI_EvaluacionPrioridadDetalle WHERE EvaluacionPrioridadId = @EvaluacionPrioridadId AND Puntuacion IS NOT NULL) <> 6
        BEGIN
            RAISERROR('La evaluacion no tiene las 6 puntuaciones completas', 16, 1);
            RETURN;
        END

        DECLARE @PuntajeTotal DECIMAL(5,2) = (
            SELECT SUM(CAST(Puntuacion AS DECIMAL(5,2)) / 5 * PesoAplicado)
            FROM dbo.TI_EvaluacionPrioridadDetalle
            WHERE EvaluacionPrioridadId = @EvaluacionPrioridadId
        );

        UPDATE dbo.TI_EvaluacionesPrioridad
           SET EstadoEvaluacion = 'COMPLETA', PuntajeTotal = @PuntajeTotal, FechaFinalizacion = SYSUTCDATETIME()
         WHERE EvaluacionPrioridadId = @EvaluacionPrioridadId;

        COMMIT TRANSACTION;
        SELECT @PuntajeTotal AS PuntajeTotal;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- BLOQUE 2: la decision inicial de prioridad ya NO depende de una
-- evaluacion de 6 criterios (TI_EvaluacionesPrioridad queda sin uso en el
-- flujo nuevo, pero no se elimina ni se borra su historico). @EvaluacionPrioridadId
-- se vuelve OPCIONAL (NULL por defecto) unicamente para conservar
-- trazabilidad si alguna decision antigua lo traia -- el flujo nuevo nunca
-- lo envia. La condicion que antes exigia "evaluacion COMPLETA" se
-- reemplaza por una validacion directa: debe existir una version de
-- alcance APROBADA y VIGENTE (sin sucesora en BORRADOR/PENDIENTE_APROBACION)
-- para la solicitud.
CREATE OR ALTER PROCEDURE dbo.sp_TI_DecisionesPrioridad_Crear
    @SolicitudId BIGINT,
    @EvaluacionPrioridadId BIGINT = NULL,
    @TipoDecision NVARCHAR(30),
    @PrioridadId INT = NULL,
    @PosicionBacklog INT = NULL,
    @Motivo NVARCHAR(2000),
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@TipoDecision IS NULL)
        BEGIN
            RAISERROR('El tipo de decision es obligatorio', 16, 1);
            RETURN;
        END
        IF (@TipoDecision NOT IN ('PRIORIZAR', 'MANTENER_BACKLOG', 'POSPONER', 'NO_PRIORIZAR', 'RECHAZAR'))
        BEGIN
            RAISERROR('Tipo de decision invalido', 16, 1);
            RETURN;
        END

        IF (@TipoDecision = 'PRIORIZAR')
        BEGIN
            IF (@PrioridadId IS NULL)
            BEGIN
                RAISERROR('La decision PRIORIZAR requiere especificar una prioridad', 16, 1);
                RETURN;
            END
            IF NOT EXISTS (SELECT 1 FROM dbo.TI_Prioridades WHERE PrioridadId = @PrioridadId)
            BEGIN
                RAISERROR('La prioridad especificada no existe', 16, 1);
                RETURN;
            END
        END

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @SolicitudId, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM dbo.TI_DecisionesPrioridad WHERE SolicitudId = @SolicitudId AND EstadoDecision = 'PENDIENTE')
        BEGIN
            RAISERROR('Ya existe una decision de prioridad pendiente de aprobacion para esta solicitud', 16, 1);
            RETURN;
        END

        DECLARE @EstadoActualCodigo NVARCHAR(100) = (
            SELECT es.Codigo FROM dbo.TI_Solicitudes s
            INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
            WHERE s.SolicitudId = @SolicitudId
        );
        IF (@EstadoActualCodigo IS NULL)
        BEGIN
            RAISERROR('Solicitud no encontrada', 16, 1);
            RETURN;
        END
        IF (@EstadoActualCodigo NOT IN ('PENDIENTE_PRIORIZACION', 'PRIORIZADA', 'EN_BACKLOG'))
        BEGIN
            RAISERROR('La solicitud no se encuentra en una fase que admita registrar una decision de prioridad', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (
            SELECT 1 FROM dbo.TI_VersionesAlcance v
            WHERE v.SolicitudId = @SolicitudId
              AND v.EstadoVersion = 'APROBADA'
              AND NOT EXISTS (
                  SELECT 1 FROM dbo.TI_VersionesAlcance suc
                  WHERE suc.VersionAnteriorId = v.VersionAlcanceId AND suc.EstadoVersion IN ('BORRADOR', 'PENDIENTE_APROBACION')
              )
        )
        BEGIN
            RAISERROR('No existe una version de alcance aprobada y vigente para esta solicitud', 16, 1);
            RETURN;
        END

        DECLARE @NumeroDecisionNueva INT = (SELECT ISNULL(MAX(NumeroDecision), 0) + 1 FROM dbo.TI_DecisionesPrioridad WHERE SolicitudId = @SolicitudId);

        -- Regla de negocio: la priorizacion INICIAL del backlog la acuerdan
        -- Jefatura de TI y Planeacion Estrategica fuera de Humaya Digital;
        -- Jefatura solo la registra, y esa decision inicial NO debe quedar
        -- pendiente de una aprobacion individual de Planeacion. Planeacion
        -- solo autoriza las REPRIORIZACIONES posteriores (una nueva decision
        -- despues de que la solicitud ya tuvo alguna vez una decision
        -- CONFIRMADA). Se detecta aqui, en el momento de crear, si ya existe
        -- una decision CONFIRMADA previa para esta solicitud.
        DECLARE @EsDecisionInicial BIT = CASE WHEN EXISTS (
            SELECT 1 FROM dbo.TI_DecisionesPrioridad WHERE SolicitudId = @SolicitudId AND EstadoDecision = 'CONFIRMADA'
        ) THEN 0 ELSE 1 END;

        INSERT INTO dbo.TI_DecisionesPrioridad (SolicitudId, EvaluacionPrioridadId, NumeroDecision, TipoDecision, PrioridadId, PosicionBacklog, EstadoDecision, Motivo, RegistradaPorUsuarioId, FechaRegistro, FechaConfirmacion)
        VALUES (@SolicitudId, @EvaluacionPrioridadId, @NumeroDecisionNueva, @TipoDecision, @PrioridadId, @PosicionBacklog,
                CASE WHEN @EsDecisionInicial = 1 THEN 'CONFIRMADA' ELSE 'PENDIENTE' END,
                @Motivo, @usuarioId, SYSUTCDATETIME(),
                CASE WHEN @EsDecisionInicial = 1 THEN SYSUTCDATETIME() ELSE NULL END);

        DECLARE @DecisionPrioridadId BIGINT = SCOPE_IDENTITY();

        IF (@EsDecisionInicial = 1)
        BEGIN
            -- Jefatura de TI es quien registra, asi que su propio registro
            -- es su aprobacion real (identidad real del usuario que llamo al
            -- SP, nunca simulada). No se inserta fila para
            -- GERENCIA_PLANEACION: no hay nada pendiente que Planeacion
            -- pueda ni deba autorizar en la decision inicial.
            INSERT INTO dbo.TI_AprobacionesPrioridad (DecisionPrioridadId, TipoAprobador, EstadoAprobacion, AprobadorUsuarioId, FechaSolicitud, FechaDecision)
            VALUES (@DecisionPrioridadId, 'JEFATURA_TI', 'APROBADA', @usuarioId, SYSUTCDATETIME(), SYSUTCDATETIME());

            DECLARE @CodigoDestinoInicial NVARCHAR(100) = CASE @TipoDecision
                WHEN 'PRIORIZAR' THEN 'PRIORIZADA'
                WHEN 'RECHAZAR' THEN 'RECHAZADA'
                ELSE 'EN_BACKLOG'
            END;

            UPDATE dbo.TI_Solicitudes
               SET PrioridadId = CASE WHEN @TipoDecision = 'PRIORIZAR' THEN @PrioridadId
                                       WHEN @TipoDecision IN ('NO_PRIORIZAR', 'RECHAZAR') THEN NULL
                                       ELSE PrioridadId END
             WHERE SolicitudId = @SolicitudId;

            IF (@CodigoDestinoInicial <> @EstadoActualCodigo)
                EXEC dbo.sp_TI_Solicitudes_CambiarEstado @SolicitudId = @SolicitudId, @EstadoNuevoCodigo = @CodigoDestinoInicial, @usuarioId = @usuarioId, @Motivo = @Motivo;
        END
        ELSE
        BEGIN
            -- Repriorizacion (ya hubo una decision CONFIRMADA antes):
            -- Jefatura de TI es quien solicita el cambio -- su registro ya
            -- expresa su posicion, igual que en la decision inicial -- y
            -- solo Planeacion Estrategica autoriza la repriorizacion
            -- (autorizacion de un solo lado, no aprobacion dual). Mientras
            -- esta fila quede PENDIENTE, la decision y programacion
            -- vigentes (la ultima CONFIRMADA) no se tocan -- ver el cambio
            -- en sp_TI_Solicitudes_ResumenSeguimiento que ya no resuelve la
            -- "vigente" como la ultima por numero sino como la ultima
            -- CONFIRMADA.
            INSERT INTO dbo.TI_AprobacionesPrioridad (DecisionPrioridadId, TipoAprobador, EstadoAprobacion, AprobadorUsuarioId, FechaSolicitud, FechaDecision)
            VALUES (@DecisionPrioridadId, 'JEFATURA_TI', 'APROBADA', @usuarioId, SYSUTCDATETIME(), SYSUTCDATETIME());

            INSERT INTO dbo.TI_AprobacionesPrioridad (DecisionPrioridadId, TipoAprobador, EstadoAprobacion, FechaSolicitud)
            VALUES (@DecisionPrioridadId, 'GERENCIA_PLANEACION', 'PENDIENTE', SYSUTCDATETIME());
        END

        COMMIT TRANSACTION;
        SELECT @DecisionPrioridadId AS DecisionPrioridadId, @NumeroDecisionNueva AS NumeroDecision;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- TipoAprobador se valida contra el rol real del usuario (JEFATURA_TI o
-- GERENCIA_PLANEACION activo) -- nunca se confia en el valor del cliente.
CREATE OR ALTER PROCEDURE dbo.sp_TI_AprobacionesPrioridad_Registrar
    @DecisionPrioridadId BIGINT,
    @TipoAprobador NVARCHAR(30),
    @EstadoDecision NVARCHAR(20),
    @Comentario NVARCHAR(2000) = NULL,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@TipoAprobador IS NULL OR @EstadoDecision IS NULL)
        BEGIN
            RAISERROR('Los parametros de la aprobacion son obligatorios', 16, 1);
            RETURN;
        END
        IF (@TipoAprobador NOT IN ('JEFATURA_TI', 'GERENCIA_PLANEACION'))
        BEGIN
            RAISERROR('Tipo de aprobador invalido', 16, 1);
            RETURN;
        END
        IF (@EstadoDecision NOT IN ('APROBADA', 'RECHAZADA'))
        BEGIN
            RAISERROR('El estado de aprobacion solicitado es invalido', 16, 1);
            RETURN;
        END

        DECLARE @LockResult INT;
        EXEC @LockResult = sp_getapplock @Resource = @DecisionPrioridadId, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
        IF (@LockResult < 0)
        BEGIN
            RAISERROR('No se pudo asegurar la operacion de forma exclusiva, intente de nuevo', 16, 1);
            RETURN;
        END

        DECLARE @SolicitudId BIGINT, @EvaluacionPrioridadId BIGINT, @TipoDecisionDecision NVARCHAR(30), @PrioridadIdDecision INT, @EstadoDecisionActual NVARCHAR(20), @MotivoDecision NVARCHAR(2000);
        SELECT @SolicitudId = SolicitudId, @EvaluacionPrioridadId = EvaluacionPrioridadId,
               @TipoDecisionDecision = TipoDecision, @PrioridadIdDecision = PrioridadId, @EstadoDecisionActual = EstadoDecision, @MotivoDecision = Motivo
        FROM dbo.TI_DecisionesPrioridad WHERE DecisionPrioridadId = @DecisionPrioridadId;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La decision de prioridad indicada no existe', 16, 1);
            RETURN;
        END
        IF (@EstadoDecisionActual <> 'PENDIENTE')
        BEGIN
            RAISERROR('La decision ya no admite aprobaciones', 16, 1);
            RETURN;
        END

        -- Re-chequeo de estado de solicitud y vigencia de version/evaluacion
        -- AL MOMENTO DE CONFIRMAR, no solo al momento de crear la decision.
        DECLARE @EstadoActualCodigo NVARCHAR(100) = (
            SELECT es.Codigo FROM dbo.TI_Solicitudes s
            INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
            WHERE s.SolicitudId = @SolicitudId
        );
        IF (@EstadoActualCodigo IS NULL)
        BEGIN
            RAISERROR('Solicitud no encontrada', 16, 1);
            RETURN;
        END
        IF (@EstadoActualCodigo NOT IN ('PENDIENTE_PRIORIZACION', 'PRIORIZADA', 'EN_BACKLOG'))
        BEGIN
            RAISERROR('La solicitud ya no se encuentra en una fase que admita esta decision', 16, 1);
            RETURN;
        END
        -- BLOQUE 2: re-chequeo de vigencia AL CONFIRMAR, ya sin depender de
        -- una evaluacion de 6 criterios (ver sp_TI_DecisionesPrioridad_Crear).
        IF NOT EXISTS (
            SELECT 1 FROM dbo.TI_VersionesAlcance v
            WHERE v.SolicitudId = @SolicitudId
              AND v.EstadoVersion = 'APROBADA'
              AND NOT EXISTS (SELECT 1 FROM dbo.TI_VersionesAlcance suc WHERE suc.VersionAnteriorId = v.VersionAlcanceId AND suc.EstadoVersion IN ('BORRADOR','PENDIENTE_APROBACION'))
        )
        BEGIN
            RAISERROR('No existe una version de alcance aprobada y vigente para esta solicitud', 16, 1);
            RETURN;
        END

        -- Autorizacion efectiva por rol real
        IF NOT EXISTS (
            SELECT 1 FROM dbo.TI_UsuarioRoles ur INNER JOIN dbo.TI_Roles r ON r.RolId = ur.RolId
            WHERE ur.UsuarioId = @usuarioId AND ur.Activo = 1 AND r.Codigo = @TipoAprobador
        )
        BEGIN
            RAISERROR('El usuario no tiene el rol requerido para este tipo de aprobador', 16, 1);
            RETURN;
        END

        DECLARE @EstadoAprobacionActual NVARCHAR(20);
        SELECT @EstadoAprobacionActual = EstadoAprobacion
        FROM dbo.TI_AprobacionesPrioridad WHERE DecisionPrioridadId = @DecisionPrioridadId AND TipoAprobador = @TipoAprobador;

        IF (@EstadoAprobacionActual IS NULL)
        BEGIN
            RAISERROR('No existe la fila de aprobacion para el tipo de aprobador indicado', 16, 1);
            RETURN;
        END
        IF (@EstadoAprobacionActual <> 'PENDIENTE')
        BEGIN
            RAISERROR('Esta aprobacion ya fue resuelta anteriormente y no puede modificarse', 16, 1);
            RETURN;
        END

        DECLARE @AprobadorLadoContrario INT = (
            SELECT AprobadorUsuarioId FROM dbo.TI_AprobacionesPrioridad
            WHERE DecisionPrioridadId = @DecisionPrioridadId AND TipoAprobador <> @TipoAprobador
        );
        IF (@usuarioId = @AprobadorLadoContrario)
        BEGIN
            RAISERROR('La misma persona no puede autorizar ambos lados', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_AprobacionesPrioridad
           SET EstadoAprobacion = @EstadoDecision, FechaDecision = SYSUTCDATETIME(), Comentario = @Comentario, AprobadorUsuarioId = @usuarioId
         WHERE DecisionPrioridadId = @DecisionPrioridadId AND TipoAprobador = @TipoAprobador;

        IF (@EstadoDecision = 'RECHAZADA')
        BEGIN
            UPDATE dbo.TI_DecisionesPrioridad SET EstadoDecision = 'RECHAZADA' WHERE DecisionPrioridadId = @DecisionPrioridadId;
            -- la fila hermana PENDIENTE no se toca
        END
        ELSE IF NOT EXISTS (SELECT 1 FROM dbo.TI_AprobacionesPrioridad WHERE DecisionPrioridadId = @DecisionPrioridadId AND EstadoAprobacion <> 'APROBADA')
        BEGIN
            UPDATE dbo.TI_DecisionesPrioridad SET EstadoDecision = 'CONFIRMADA', FechaConfirmacion = SYSUTCDATETIME() WHERE DecisionPrioridadId = @DecisionPrioridadId;

            DECLARE @CodigoDestino NVARCHAR(100) = CASE @TipoDecisionDecision
                WHEN 'PRIORIZAR' THEN 'PRIORIZADA'
                WHEN 'RECHAZAR' THEN 'RECHAZADA'
                ELSE 'EN_BACKLOG'
            END;

            -- BLOQUE 2: PuntajePrioridadActual ya no se deriva de una
            -- evaluacion de 6 criterios (dejo de ser requisito); se limpia
            -- a NULL junto con PrioridadId cuando corresponde, y no se
            -- vuelve a escribir con un puntaje para PRIORIZAR.
            UPDATE dbo.TI_Solicitudes
               SET PrioridadId = CASE WHEN @TipoDecisionDecision = 'PRIORIZAR' THEN @PrioridadIdDecision
                                       WHEN @TipoDecisionDecision IN ('NO_PRIORIZAR', 'RECHAZAR') THEN NULL
                                       ELSE PrioridadId END,
                   PuntajePrioridadActual = CASE WHEN @TipoDecisionDecision IN ('NO_PRIORIZAR', 'RECHAZAR') THEN NULL
                                                  ELSE PuntajePrioridadActual END
             WHERE SolicitudId = @SolicitudId;

            IF (@CodigoDestino <> @EstadoActualCodigo)
                EXEC dbo.sp_TI_Solicitudes_CambiarEstado @SolicitudId = @SolicitudId, @EstadoNuevoCodigo = @CodigoDestino, @usuarioId = @usuarioId, @Motivo = @MotivoDecision;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
