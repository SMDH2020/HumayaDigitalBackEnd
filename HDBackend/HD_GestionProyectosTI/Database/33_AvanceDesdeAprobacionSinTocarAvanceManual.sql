-- ─────────────────────────────────────────────────────────────────────────
-- Corrige el enfoque de 32_AprobarRevisionMarcaTerminada.sql: el usuario
-- aclaro que AvanceManual y "revision aprobada" son dos cosas distintas y
-- no deben mezclarse -- AvanceManual es el numero que Jefatura edita a mano
-- en Backlog Mensual; "terminada" es un estado que se deriva de que el
-- SOLICITANTE haya aprobado la revision de esa funcionalidad (flujo:
-- Jefatura manda a revision cuando el desarrollo ya se terminto -> el
-- solicitante aprueba o rechaza). El script 32 pisaba AvanceManual=100 al
-- aprobar, lo cual mezclaba ambos conceptos en la misma columna.
--
-- 1) sp_TI_Funcionalidades_ResolverRevision regresa a NO tocar
--    AvanceManual (mismo cuerpo que 30_FixEstadoSolicitudDesactualizado.sql,
--    sin el UPDATE que agrego el script 32).
--
-- 2) sp_TI_Solicitudes_Listado calcula el avance de cada funcionalidad como
--    EfectivoAvance = 100 si su revision vigente esta APROBADA (mismo
--    criterio "aprobacion vigente" que sp_TI_Solicitudes_RecalcularEstadoDerivado:
--    ultimo cambio de esa funcionalidad, EstadoCambio='APROBADO', y su
--    Mes/AnioPropuesto coincide con el Mes/AnioProgramado ACTUAL), o
--    AvanceManual en cualquier otro caso. AvanceManual como columna NUNCA
--    se modifica por esto -- solo se usa en el calculo de lectura de la
--    Bandeja.
-- ─────────────────────────────────────────────────────────────────────────

CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_ResolverRevision
    @CambioProgramacionId BIGINT,
    @EstadoResolucion NVARCHAR(20),   -- 'APROBADO' | 'RECHAZADO'
    @ComentarioResolucion NVARCHAR(2000) = NULL,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@EstadoResolucion IS NULL OR @EstadoResolucion NOT IN ('APROBADO', 'RECHAZADO'))
        BEGIN
            RAISERROR('El estado de resolucion debe ser APROBADO o RECHAZADO', 16, 1);
            RETURN;
        END
        IF (@EstadoResolucion = 'RECHAZADO' AND (@ComentarioResolucion IS NULL OR LTRIM(RTRIM(@ComentarioResolucion)) = ''))
        BEGIN
            RAISERROR('El motivo es obligatorio para rechazar la revision', 16, 1);
            RETURN;
        END

        DECLARE @SolicitudId BIGINT, @FuncionalidadId BIGINT, @SolicitanteUsuarioId INT;
        SELECT @SolicitudId = c.SolicitudId, @FuncionalidadId = c.FuncionalidadId
        FROM dbo.TI_CambiosProgramacion c
        WHERE c.CambioProgramacionId = @CambioProgramacionId;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La revision indicada no existe', 16, 1);
            RETURN;
        END

        SELECT @SolicitanteUsuarioId = s.SolicitanteUsuarioId FROM dbo.TI_Solicitudes s WHERE s.SolicitudId = @SolicitudId;

        IF (@usuarioId <> @SolicitanteUsuarioId)
        BEGIN
            RAISERROR('Solo el usuario solicitante de esta solicitud puede resolver la revision de la funcionalidad', 16, 1);
            RETURN;
        END

        DECLARE @Resuelto TABLE (FuncionalidadId BIGINT);

        UPDATE dbo.TI_CambiosProgramacion
           SET EstadoCambio = @EstadoResolucion, DecididoPorUsuarioId = @usuarioId, FechaDecision = SYSUTCDATETIME(), ComentarioResolucion = @ComentarioResolucion
        OUTPUT inserted.FuncionalidadId
        INTO @Resuelto
         WHERE CambioProgramacionId = @CambioProgramacionId AND EstadoCambio = 'PENDIENTE';

        IF (SELECT COUNT(*) FROM @Resuelto) = 0
        BEGIN
            RAISERROR('La revision no existe o ya fue resuelta anteriormente', 16, 1);
            RETURN;
        END

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)),
                CASE WHEN @EstadoResolucion = 'APROBADO' THEN 'FUNCIONALIDAD_REVISION_APROBADA' ELSE 'FUNCIONALIDAD_REVISION_RECHAZADA' END,
                SYSUTCDATETIME(), NULL, @ComentarioResolucion, 'API');

        EXEC dbo.sp_TI_Solicitudes_RecalcularEstadoDerivado @SolicitudId = @SolicitudId, @usuarioId = @usuarioId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Listado
    @usuarioId      INT,
    @verTodo        BIT,
    @estadoCodigo   NVARCHAR(100)  = NULL,
    @tipoCodigo     NVARCHAR(80)   = NULL,
    @fechaInicio    DATE           = NULL,
    @fechaFin       DATE           = NULL,
    @areas          NVARCHAR(MAX)  = NULL  -- areas solicitantes separadas por coma; NULL o vacio = todas
AS
BEGIN
    SET NOCOUNT ON;

    -- Nombre real del solicitante\creador (ver 17_NombresRealesUsuarios.sql):
    -- UsuarioId = AppMH.dbo.Empleados.IDEmpleado, mismo servidor SQL.
    SELECT
        s.SolicitudId, s.Folio, s.Titulo, s.DescripcionNecesidad, s.ProblemaActual, s.BeneficioEsperado,
        s.TipoSolicitudId, ts.Codigo AS TipoSolicitudCodigo, ts.Nombre AS TipoSolicitud,
        s.EstadoSolicitudId, es.Codigo AS EstadoCodigo, es.Nombre AS EstadoSolicitud, es.Etapa,
        s.PrioridadId, p.Codigo AS PrioridadCodigo, p.Nombre AS Prioridad,
        s.SolicitanteUsuarioId, s.CreadaPorUsuarioId, s.AreaSolicitante, s.EsConfidencial,
        s.MotivoRechazo, s.MotivoCancelacion,
        s.FechaCreacion, s.FechaEnvio, s.FechaUltimaActualizacion, s.FechaCierre,
        CONCAT(empSol.Nombre, ' ', empSol.ApellidoPaterno, ' ', empSol.ApellidoMaterno) AS SolicitanteNombre,
        CONCAT(empCrea.Nombre, ' ', empCrea.ApellidoPaterno, ' ', empCrea.ApellidoMaterno) AS CreadaPorNombre,
        empSol.Correo AS SolicitanteCorreo,
        agg.TotalDiasEstimados, agg.FuncionalidadesTotal, agg.SumaDiasPorAvance,
        aprob.FuncionalidadesAprobadas,
        meses.MesesPriorizados
    FROM dbo.TI_Solicitudes s
    INNER JOIN dbo.TI_TiposSolicitud ts ON ts.TipoSolicitudId = s.TipoSolicitudId
    INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
    LEFT JOIN dbo.TI_Prioridades p ON p.PrioridadId = s.PrioridadId
    LEFT JOIN AppMH.dbo.Empleados empSol ON empSol.IDEmpleado = s.SolicitanteUsuarioId AND empSol.Estatus = 1
    LEFT JOIN AppMH.dbo.Empleados empCrea ON empCrea.IDEmpleado = s.CreadaPorUsuarioId AND empCrea.Estatus = 1
    OUTER APPLY (
        SELECT TOP 1 va.VersionAlcanceId
        FROM dbo.TI_VersionesAlcance va
        WHERE va.SolicitudId = s.SolicitudId AND va.EstadoVersion = 'APROBADA'
        ORDER BY va.NumeroVersion DESC
    ) vig
    OUTER APPLY (
        -- EfectivoAvance por funcionalidad = 100 si su revision vigente
        -- esta APROBADA por el solicitante (terminada), o AvanceManual en
        -- cualquier otro caso -- AvanceManual (columna) nunca se modifica,
        -- esto solo afecta como se SUMA el avance de la Bandeja.
        SELECT
            SUM(f.DiasEstimados) AS TotalDiasEstimados,
            COUNT(*) AS FuncionalidadesTotal,
            SUM(f.DiasEstimados * (
                CASE WHEN uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado
                     THEN 100
                     ELSE ISNULL(f.AvanceManual, 0)
                END
            )) AS SumaDiasPorAvance
        FROM dbo.TI_Funcionalidades f
        OUTER APPLY (
            SELECT TOP 1 c.EstadoCambio, c.MesPropuesto, c.AnioPropuesto
            FROM dbo.TI_CambiosProgramacion c
            WHERE c.FuncionalidadId = f.FuncionalidadId
            ORDER BY c.FechaSolicitud DESC
        ) uc
        WHERE f.VersionAlcanceId = vig.VersionAlcanceId AND f.Activa = 1
    ) agg
    OUTER APPLY (
        -- "Priorizada" = ya tiene Mes/Anio asignado, sin importar si ya se
        -- aprobo/termino -- misma definicion que PriorizacionFuncionalidadTI.js.
        -- Esto NO es avance real (ver SumaDiasPorAvance arriba).
        SELECT COUNT(*) AS FuncionalidadesAprobadas
        FROM dbo.TI_Funcionalidades f
        WHERE f.VersionAlcanceId = vig.VersionAlcanceId AND f.Activa = 1
          AND f.MesProgramado IS NOT NULL AND f.AnioProgramado IS NOT NULL
    ) aprob
    OUTER APPLY (
        SELECT STRING_AGG(CONCAT(mesesDistintos.AnioProgramado, '-', mesesDistintos.MesProgramado), ',')
                 WITHIN GROUP (ORDER BY mesesDistintos.AnioProgramado, mesesDistintos.MesProgramado) AS MesesPriorizados
        FROM (
            SELECT DISTINCT f.AnioProgramado, f.MesProgramado
            FROM dbo.TI_Funcionalidades f
            WHERE f.VersionAlcanceId = vig.VersionAlcanceId AND f.Activa = 1
              AND f.MesProgramado IS NOT NULL AND f.AnioProgramado IS NOT NULL
        ) mesesDistintos
    ) meses
    WHERE (@estadoCodigo IS NULL OR es.Codigo = @estadoCodigo)
      AND (@tipoCodigo IS NULL OR ts.Codigo = @tipoCodigo)
      AND (@fechaInicio IS NULL OR CAST(s.FechaCreacion AS DATE) >= @fechaInicio)
      AND (@fechaFin IS NULL OR CAST(s.FechaCreacion AS DATE) <= @fechaFin)
      AND (@areas IS NULL OR @areas = '' OR s.AreaSolicitante IN (SELECT value FROM STRING_SPLIT(@areas, ',')))
      AND (
            @verTodo = 1
            OR s.SolicitanteUsuarioId = @usuarioId
            OR s.CreadaPorUsuarioId = @usuarioId
            OR EXISTS (
                SELECT 1 FROM dbo.TI_SolicitudParticipantes sp
                WHERE sp.SolicitudId = s.SolicitudId AND sp.UsuarioId = @usuarioId AND sp.Activo = 1
            )
          )
    ORDER BY s.FechaCreacion DESC;
END
GO
