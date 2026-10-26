-- ============================================================================
-- Bug reportado: una solicitud con sus 2 funcionalidades ya priorizadas Y
-- aprobadas por el solicitante seguia mostrando "Pendiente de priorizacion".
--
-- Causa real: el estado general de la solicitud (TI_Solicitudes.EstadoSolicitudId)
-- es un valor ALMACENADO que sp_TI_Funcionalidades_ResolverRevision recalcula
-- y GUARDA en el momento de aprobar una revision -- pero nada mas lo vuelve a
-- recalcular despues. Si Jefatura reprograma una funcionalidad ya aprobada
-- (sp_TI_Funcionalidades_ProgramarInicial, caso REPROGRAMACION) el mes/anio
-- cambia y la aprobacion anterior deja de aplicar (regla ya documentada en
-- 08_Migracion_ProgramacionMensual.sql: MesPropuesto/AnioPropuesto vs. el
-- MesProgramado/AnioProgramado VIGENTE) -- pero el estado guardado de la
-- solicitud se queda congelado en lo que sea que haya calculado la ULTIMA
-- aprobacion, sin reflejar la realidad actual. Con 2 funcionalidades, el
-- orden tipico -- aprobar A, reprogramar A, aprobar B -- deja el conteo
-- calculado en el momento de aprobar B sin contar a A (su aprobacion vieja ya
-- no matcheaba su nuevo mes), y si ese conteo da 0 el bloque ni siquiera
-- actualiza el estado (@EstadoSolicitudDerivado se queda NULL).
--
-- Fix: se extrae el calculo a un procedimiento compartido
-- (sp_TI_Solicitudes_RecalcularEstadoDerivado) que SIEMPRE recalcula sobre
-- los datos actuales (no un snapshot del momento de aprobar) y se llama al
-- final de las TRES operaciones que pueden invalidar o completar el avance:
-- ResolverRevision (aprobar), ProgramarInicial (programar/reprogramar) y
-- RegresarABacklog (quitar). Tambien permite retroceder el estado (de
-- PRIORIZADA/TERMINADA de vuelta a PENDIENTE_PRIORIZACION) cuando una
-- reprogramacion invalida todas las aprobaciones vigentes -- antes el
-- estado solo podia avanzar, nunca reflejar un retroceso real.
-- ============================================================================

CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_RecalcularEstadoDerivado
    @SolicitudId BIGINT,
    @usuarioId   INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @VersionVigenteId BIGINT = (
        SELECT TOP 1 VersionAlcanceId FROM dbo.TI_VersionesAlcance
        WHERE SolicitudId = @SolicitudId AND EstadoVersion = 'APROBADA'
        ORDER BY NumeroVersion DESC
    );

    IF (@VersionVigenteId IS NULL) RETURN;

    DECLARE @TotalActivas INT = (
        SELECT COUNT(*) FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId = @VersionVigenteId AND Activa = 1
    );

    IF (@TotalActivas = 0) RETURN;

    -- "Priorizada" = tiene Mes/Anio asignado, SIN importar si ya tiene (o
    -- necesita) aprobacion del solicitante -- misma definicion que ya usa
    -- el resto de la app (PriorizacionFuncionalidadTI.js, y la columna
    -- "Priorizado"/"Funcionalidades Priorizadas" de la Bandeja, ver
    -- 26_FixFuncionalidadesPriorizadasDefinicion.sql). Antes este
    -- procedimiento exigia ademas la aprobacion para avanzar el estado
    -- general -- de ahi que una solicitud con sus funcionalidades YA
    -- priorizadas (con o sin aprobacion) se quedara en
    -- "Pendiente de priorizacion" indefinidamente.
    DECLARE @TotalPriorizadas INT = (
        SELECT COUNT(*) FROM dbo.TI_Funcionalidades
        WHERE VersionAlcanceId = @VersionVigenteId AND Activa = 1
          AND MesProgramado IS NOT NULL AND AnioProgramado IS NOT NULL
    );

    -- La aprobacion del solicitante solo se sigue usando para el estado mas
    -- avanzado (TERMINADA = todas aprobadas). Mientras no se cumpla eso pero
    -- ya esten todas priorizadas (independientemente del mes de cada una),
    -- el estado es EN_BACKLOG -- ya no existe un estado intermedio
    -- "PRIORIZADA" en este calculo.
    DECLARE @TotalAprobadas INT;
    ;WITH UltimoCambio AS (
        SELECT c.*, ROW_NUMBER() OVER (PARTITION BY c.FuncionalidadId ORDER BY c.FechaSolicitud DESC) AS Orden_
        FROM dbo.TI_CambiosProgramacion c
        WHERE c.SolicitudId = @SolicitudId
    )
    SELECT @TotalAprobadas = COUNT(*)
    FROM dbo.TI_Funcionalidades f
    INNER JOIN UltimoCambio uc ON uc.FuncionalidadId = f.FuncionalidadId AND uc.Orden_ = 1
    WHERE f.VersionAlcanceId = @VersionVigenteId AND f.Activa = 1
      AND uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado;

    DECLARE @EstadoActualSolId INT, @EstadoActualSolCodigo NVARCHAR(100);
    SELECT @EstadoActualSolId = s.EstadoSolicitudId, @EstadoActualSolCodigo = es.Codigo
    FROM dbo.TI_Solicitudes s
    INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
    WHERE s.SolicitudId = @SolicitudId;

    -- Solo se reevalua entre los estados de esta etapa del flujo (las mismas
    -- que habilitan los paneles de alcance/prioridad en el front, ver
    -- ESTADOS_CON_PANELES_FASE2B) -- nunca pisa RECHAZADA/CANCELADA ni
    -- estados anteriores a tener version de alcance aprobada.
    IF (@EstadoActualSolCodigo NOT IN ('PENDIENTE_PRIORIZACION', 'EN_REVISION_PRIORIDAD', 'PRIORIZADA', 'TERMINADA', 'EN_BACKLOG'))
        RETURN;

    DECLARE @EstadoSolicitudDerivado NVARCHAR(100);
    IF (@TotalAprobadas = @TotalActivas)
        SET @EstadoSolicitudDerivado = 'TERMINADA';
    ELSE IF (@TotalPriorizadas = @TotalActivas)
        SET @EstadoSolicitudDerivado = 'EN_BACKLOG';
    ELSE
        SET @EstadoSolicitudDerivado = 'PENDIENTE_PRIORIZACION';

    IF (@EstadoActualSolCodigo = @EstadoSolicitudDerivado) RETURN;

    DECLARE @EstadoSolicitudDerivadoId INT = (SELECT EstadoSolicitudId FROM dbo.TI_EstadosSolicitud WHERE Codigo = @EstadoSolicitudDerivado);
    IF (@EstadoSolicitudDerivadoId IS NULL) RETURN;

    UPDATE dbo.TI_Solicitudes SET EstadoSolicitudId = @EstadoSolicitudDerivadoId, FechaUltimaActualizacion = SYSUTCDATETIME() WHERE SolicitudId = @SolicitudId;

    INSERT INTO dbo.TI_SolicitudEstadosHistorial (SolicitudId, EstadoAnteriorId, EstadoNuevoId, UsuarioCambioId, FechaCambio, Motivo, Origen)
    VALUES (@SolicitudId, @EstadoActualSolId, @EstadoSolicitudDerivadoId, @usuarioId, SYSUTCDATETIME(),
            CASE WHEN @EstadoSolicitudDerivado = 'TERMINADA' THEN 'Todas las funcionalidades vigentes fueron aprobadas'
                 WHEN @EstadoSolicitudDerivado = 'EN_BACKLOG' THEN CONCAT('Funcionalidades priorizadas (mes/anio asignado): ', @TotalPriorizadas, ' de ', @TotalActivas)
                 ELSE CONCAT('Funcionalidades priorizadas: ', @TotalPriorizadas, ' de ', @TotalActivas, ' -- ya no cumple ninguna de las condiciones para EN_BACKLOG/TERMINADA') END,
            'API');

    INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
    VALUES (@SolicitudId, @usuarioId, 'TI_Solicitudes', CAST(@SolicitudId AS NVARCHAR(200)), 'CAMBIAR_ESTADO', SYSUTCDATETIME(), @EstadoActualSolCodigo, @EstadoSolicitudDerivado, 'API');
END
GO

-- ----------------------------------------------------------------------------
-- sp_TI_Funcionalidades_ResolverRevision: mismo cuerpo que
-- 16_EstadoSolicitudPorAprobacionFuncionalidad.sql, reemplazando el bloque
-- de calculo inline por una llamada al procedimiento compartido. Tambien se
-- recalcula en el camino RECHAZADO (antes solo corria en APROBADO) por si
-- un rechazo quita la ultima aprobacion vigente que sostenia PRIORIZADA.
-- ----------------------------------------------------------------------------
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

-- ----------------------------------------------------------------------------
-- sp_TI_Funcionalidades_ProgramarInicial: mismo cuerpo que
-- 13_RevisionPorFuncionalidad.sql, agregando la llamada al recalculo al
-- final (una reprogramacion puede invalidar la aprobacion vigente que
-- sostenia el estado actual).
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_ProgramarInicial
    @FuncionalidadId BIGINT,
    @MesProgramado TINYINT,
    @AnioProgramado SMALLINT,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@MesProgramado IS NULL OR @MesProgramado NOT BETWEEN 1 AND 12)
        BEGIN
            RAISERROR('El mes programado debe estar entre 1 y 12', 16, 1);
            RETURN;
        END
        IF (@AnioProgramado IS NULL)
        BEGIN
            RAISERROR('El anio programado es obligatorio', 16, 1);
            RETURN;
        END

        DECLARE @SolicitudId BIGINT, @MesActual TINYINT, @AnioActual SMALLINT, @VersionAlcanceId BIGINT;
        SELECT @SolicitudId = v.SolicitudId, @MesActual = f.MesProgramado, @AnioActual = f.AnioProgramado, @VersionAlcanceId = f.VersionAlcanceId
        FROM dbo.TI_Funcionalidades f
        INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
        WHERE f.FuncionalidadId = @FuncionalidadId AND f.Activa = 1;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La funcionalidad indicada no existe o no esta activa', 16, 1);
            RETURN;
        END

        DECLARE @VersionVigenteId BIGINT = (
            SELECT TOP 1 VersionAlcanceId FROM dbo.TI_VersionesAlcance
            WHERE SolicitudId = @SolicitudId AND EstadoVersion = 'APROBADA'
            ORDER BY NumeroVersion DESC
        );
        IF (@VersionVigenteId IS NULL OR @VersionAlcanceId <> @VersionVigenteId)
        BEGIN
            RAISERROR('La funcionalidad no pertenece a la version de alcance vigente de la solicitud', 16, 1);
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM dbo.TI_CambiosProgramacion WHERE FuncionalidadId = @FuncionalidadId AND EstadoCambio = 'PENDIENTE')
        BEGIN
            RAISERROR('La funcionalidad tiene una revision pendiente; no se puede modificar su programacion', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_Funcionalidades
           SET MesProgramado = @MesProgramado, AnioProgramado = @AnioProgramado
         WHERE FuncionalidadId = @FuncionalidadId;

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)),
                CASE WHEN @MesActual IS NULL AND @AnioActual IS NULL THEN 'PROGRAMACION_INICIAL' ELSE 'REPROGRAMACION' END,
                SYSUTCDATETIME(),
                CASE WHEN @MesActual IS NULL AND @AnioActual IS NULL THEN NULL ELSE CONCAT('Mes=', @MesActual, ' Anio=', @AnioActual) END,
                CONCAT('Mes=', @MesProgramado, ' Anio=', @AnioProgramado), 'API');

        EXEC dbo.sp_TI_Solicitudes_RecalcularEstadoDerivado @SolicitudId = @SolicitudId, @usuarioId = @usuarioId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- sp_TI_Funcionalidades_RegresarABacklog: mismo cuerpo que
-- 13_RevisionPorFuncionalidad.sql, agregando la llamada al recalculo al
-- final (quitar una funcionalidad del periodo tambien puede invalidar la
-- aprobacion vigente que sostenia el estado actual).
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_RegresarABacklog
    @FuncionalidadId BIGINT,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @SolicitudId BIGINT, @MesActual TINYINT, @AnioActual SMALLINT;
        SELECT @SolicitudId = v.SolicitudId, @MesActual = f.MesProgramado, @AnioActual = f.AnioProgramado
        FROM dbo.TI_Funcionalidades f
        INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
        WHERE f.FuncionalidadId = @FuncionalidadId AND f.Activa = 1;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La funcionalidad indicada no existe o no esta activa', 16, 1);
            RETURN;
        END
        IF (@MesActual IS NULL AND @AnioActual IS NULL)
        BEGIN
            RAISERROR('La funcionalidad ya esta en backlog', 16, 1);
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM dbo.TI_CambiosProgramacion WHERE FuncionalidadId = @FuncionalidadId AND EstadoCambio = 'PENDIENTE')
        BEGIN
            RAISERROR('La funcionalidad tiene una revision pendiente; no se puede modificar su programacion', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_Funcionalidades SET MesProgramado = NULL, AnioProgramado = NULL WHERE FuncionalidadId = @FuncionalidadId;

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)), 'REGRESO_A_BACKLOG', SYSUTCDATETIME(),
                CONCAT('Mes=', @MesActual, ' Anio=', @AnioActual), NULL, 'API');

        EXEC dbo.sp_TI_Solicitudes_RecalcularEstadoDerivado @SolicitudId = @SolicitudId, @usuarioId = @usuarioId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- sp_TI_Backlog_Buscar: la exclusion "ya terminada" (AvanceManual=100) no
-- debe aplicar a funcionalidades que YA NO estan programadas en ningun mes
-- (el caso de "la quito del backlog y ya no la encuentro para priorizarla de
-- nuevo" -- Quitar/RegresarABacklog no resetea AvanceManual, y antes de este
-- fix eso la dejaba invisible para siempre en el buscador). Si sigue
-- programada en otro periodo, se mantiene la exclusion (no tiene caso
-- ofrecerla si ya esta terminada ahi).
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Backlog_Buscar
    @Mes       TINYINT,
    @Anio      SMALLINT,
    @Busqueda  NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH UltimoCambio AS (
        SELECT c.*, ROW_NUMBER() OVER (PARTITION BY c.FuncionalidadId ORDER BY c.FechaSolicitud DESC) AS Orden_
        FROM dbo.TI_CambiosProgramacion c
    )
    SELECT f.FuncionalidadId, f.Codigo, f.Nombre, f.DiasEstimados, f.AvanceManual,
           f.MesProgramado, f.AnioProgramado,
           s.SolicitudId, s.Folio, s.Titulo, s.AreaSolicitante,
           uc.CambioProgramacionId AS RevisionCambioProgramacionId,
           CASE
               WHEN uc.EstadoCambio = 'PENDIENTE' THEN 'EN_REVISION'
               WHEN uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado THEN 'APROBADA'
               WHEN uc.EstadoCambio = 'RECHAZADO' AND uc.MesAnteriorProgramado = f.MesProgramado AND uc.AnioAnteriorProgramado = f.AnioProgramado THEN 'RECHAZADA'
               ELSE NULL
           END AS RevisionEstado
    FROM dbo.TI_Funcionalidades f
    INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId AND v.EstadoVersion = 'APROBADA'
    INNER JOIN dbo.TI_Solicitudes s ON s.SolicitudId = v.SolicitudId
    LEFT JOIN UltimoCambio uc ON uc.FuncionalidadId = f.FuncionalidadId AND uc.Orden_ = 1
    WHERE f.Activa = 1
      AND (f.MesProgramado IS NULL OR f.AvanceManual IS NULL OR f.AvanceManual < 100)
      AND (f.MesProgramado IS NULL OR f.AnioProgramado IS NULL OR f.MesProgramado <> @Mes OR f.AnioProgramado <> @Anio)
      AND NOT (ISNULL(uc.EstadoCambio, '') = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado)
      AND (
            @Busqueda IS NULL OR LTRIM(RTRIM(@Busqueda)) = ''
            OR f.Nombre LIKE '%' + @Busqueda + '%'
            OR f.Codigo LIKE '%' + @Busqueda + '%'
            OR s.AreaSolicitante LIKE '%' + @Busqueda + '%'
          )
    ORDER BY s.AreaSolicitante, f.Nombre;
END
GO
