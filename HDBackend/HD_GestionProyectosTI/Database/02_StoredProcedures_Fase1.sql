USE HumayaDigital_GestionProyectosTI;
GO

/* ============================================================================
   ROLES
   ============================================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_RolTI_Obtener
    @idusuario INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL((SELECT rol FROM dbo.RolesTI WHERE idusuario = @idusuario), 'Usuario') AS rol;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_RolTI_Asignar
    @idusuario   INT,
    @rol         VARCHAR(20),   -- 'Developer' o 'Admin'
    @asignadoPor INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.RolesTI WHERE idusuario = @idusuario)
        UPDATE dbo.RolesTI
           SET rol = @rol, fechaAsignacion = GETDATE(), asignadoPor = @asignadoPor
         WHERE idusuario = @idusuario;
    ELSE
        INSERT INTO dbo.RolesTI (idusuario, rol, asignadoPor)
        VALUES (@idusuario, @rol, @asignadoPor);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_RolTI_Listado
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idusuario, rol, fechaAsignacion, asignadoPor FROM dbo.RolesTI;
END
GO

/* ============================================================================
   SOLICITUDES
   ============================================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Solicitudes_Crear
    @tipo                        VARCHAR(20),
    @titulo                      VARCHAR(200),
    @descripcion                 VARCHAR(MAX),
    @idmodulo                    INT,
    @idusuario_solicitante       INT,
    @pregunta_informacion        VARCHAR(MAX),
    @pregunta_objetivo_negocio   VARCHAR(MAX),
    @pregunta_decisiones         VARCHAR(MAX),
    @pregunta_frecuencia         VARCHAR(200),
    @pregunta_uso_compartido     VARCHAR(MAX),
    @impacto_control_interno     BIT = 0,
    @impacto_normativo           BIT = 0,
    @impacto_financiero          BIT = 0,
    @impacto_comentario          VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Solicitudes (
        tipo, titulo, descripcion, idmodulo, idusuario_solicitante,
        pregunta_informacion, pregunta_objetivo_negocio, pregunta_decisiones,
        pregunta_frecuencia, pregunta_uso_compartido,
        impacto_control_interno, impacto_normativo, impacto_financiero, impacto_comentario,
        estado, creado_por
    )
    VALUES (
        @tipo, @titulo, @descripcion, @idmodulo, @idusuario_solicitante,
        @pregunta_informacion, @pregunta_objetivo_negocio, @pregunta_decisiones,
        @pregunta_frecuencia, @pregunta_uso_compartido,
        @impacto_control_interno, @impacto_normativo, @impacto_financiero, @impacto_comentario,
        'Nueva', @idusuario_solicitante
    );

    DECLARE @idsolicitud INT = SCOPE_IDENTITY();

    INSERT INTO dbo.Bitacora (entidad, identidad, idusuario, campo, valor_anterior, valor_nuevo, tipo_evento)
    VALUES ('Solicitud', @idsolicitud, @idusuario_solicitante, 'estado', NULL, 'Nueva', 'Creacion');

    SELECT @idsolicitud AS idsolicitud;
END
GO

-- @rol: 'Usuario' | 'Developer' | 'Admin'. Usuario ve solo lo suyo; Developer ve
-- solo solicitudes con actividades asignadas a él; Admin ve todo.
CREATE OR ALTER PROCEDURE dbo.sp_Solicitudes_Listado
    @idusuario INT,
    @rol       VARCHAR(20),
    @estado    VARCHAR(40) = NULL,
    @tipo      VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.idsolicitud,
        'TI-' + RIGHT('000000' + CAST(s.idsolicitud AS VARCHAR(6)), 6) AS folio,
        s.tipo, s.titulo, s.idmodulo, s.idusuario_solicitante,
        s.estado, s.prioridad, s.fecha_estimada, s.fecha_comprometida,
        s.fecha_creacion, s.fecha_actualizacion,
        s.rondas_rechazo_alcance,
        ISNULL((SELECT SUM(estimacion_horas) FROM dbo.Actividades a WHERE a.idsolicitud = s.idsolicitud), 0) AS horas_totales,
        ISNULL((SELECT SUM(estimacion_horas) FROM dbo.Actividades a WHERE a.idsolicitud = s.idsolicitud AND a.estado = 'Terminada'), 0) AS horas_terminadas
    FROM dbo.Solicitudes s
    WHERE (@estado IS NULL OR s.estado = @estado)
      AND (@tipo IS NULL OR s.tipo = @tipo)
      AND (
            @rol = 'Admin'
            OR (@rol = 'Usuario' AND s.idusuario_solicitante = @idusuario)
            OR (@rol = 'Developer' AND (
                    s.idusuario_solicitante = @idusuario
                    OR EXISTS (SELECT 1 FROM dbo.Actividades a WHERE a.idsolicitud = s.idsolicitud AND a.idusuario_developer = @idusuario)
                ))
          )
    ORDER BY s.fecha_creacion DESC;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Solicitudes_Obtener
    @idsolicitud INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.*,
        'TI-' + RIGHT('000000' + CAST(s.idsolicitud AS VARCHAR(6)), 6) AS folio,
        ISNULL((SELECT SUM(estimacion_horas) FROM dbo.Actividades a WHERE a.idsolicitud = s.idsolicitud), 0) AS horas_totales,
        ISNULL((SELECT SUM(estimacion_horas) FROM dbo.Actividades a WHERE a.idsolicitud = s.idsolicitud AND a.estado = 'Terminada'), 0) AS horas_terminadas
    FROM dbo.Solicitudes s
    WHERE s.idsolicitud = @idsolicitud;

    SELECT idactividad, idsolicitud, descripcion, estimacion_horas, idusuario_developer,
           estado, fecha_inicio_real, fecha_fin_real, fecha_creacion
    FROM dbo.Actividades
    WHERE idsolicitud = @idsolicitud
    ORDER BY idactividad;
END
GO

-- Cambios de estado que NO requieren datos adicionales (revisar, aceptar,
-- pasar a definicion, pasar a pruebas, etc.). Para rechazo/cancelacion se
-- exige @motivo. Cancelada y Rechazada aceptan motivo obligatorio validado
-- también a nivel aplicación (el API debe rechazar la llamada si falta).
CREATE OR ALTER PROCEDURE dbo.sp_Solicitudes_CambiarEstado
    @idsolicitud   INT,
    @estado_nuevo  VARCHAR(40),
    @idusuario     INT,
    @motivo        VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @estado_anterior VARCHAR(40);
    SELECT @estado_anterior = estado FROM dbo.Solicitudes WHERE idsolicitud = @idsolicitud;

    IF @estado_anterior IS NULL
    BEGIN
        RAISERROR('Solicitud no encontrada', 16, 1);
        RETURN;
    END

    UPDATE dbo.Solicitudes
       SET estado = @estado_nuevo,
           rondas_rechazo_alcance = CASE
                WHEN @estado_nuevo = 'En definición' AND @estado_anterior = 'Pendiente de aprobación del usuario'
                THEN rondas_rechazo_alcance + 1
                ELSE rondas_rechazo_alcance
           END,
           fecha_actualizacion = GETDATE(),
           actualizado_por = @idusuario
     WHERE idsolicitud = @idsolicitud;

    INSERT INTO dbo.Bitacora (entidad, identidad, idusuario, campo, valor_anterior, valor_nuevo, tipo_evento, motivo)
    VALUES (
        'Solicitud', @idsolicitud, @idusuario, 'estado', @estado_anterior, @estado_nuevo,
        CASE
            WHEN @estado_nuevo = 'Rechazada' THEN 'RechazoRevision'
            WHEN @estado_nuevo = 'En definición' AND @estado_anterior = 'Pendiente de aprobación del usuario' THEN 'RechazoAlcance'
            WHEN @estado_nuevo = 'Cancelada' THEN 'Cancelacion'
            ELSE 'CambioEstado'
        END,
        @motivo
    );
END
GO

-- Fija prioridad y fecha estimada. Si la solicitud ya había sido priorizada
-- antes, el estado resultante es 'Repriorizada' y el motivo es obligatorio
-- (se valida en el API).
CREATE OR ALTER PROCEDURE dbo.sp_Solicitudes_Priorizar
    @idsolicitud             INT,
    @prioridad               VARCHAR(10),
    @fecha_estimada          DATE,
    @priorizado_con          VARCHAR(200),
    @comentario_priorizacion VARCHAR(MAX),
    @idusuario               INT,
    @motivo                  VARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @estado_anterior VARCHAR(40), @prioridad_anterior VARCHAR(10);
    SELECT @estado_anterior = estado, @prioridad_anterior = prioridad
    FROM dbo.Solicitudes WHERE idsolicitud = @idsolicitud;

    DECLARE @yaPriorizada BIT = CASE WHEN @estado_anterior IN
        ('Priorizada','Repriorizada','Asignada','En progreso','En pruebas') THEN 1 ELSE 0 END;

    UPDATE dbo.Solicitudes
       SET prioridad = @prioridad,
           fecha_estimada = @fecha_estimada,
           priorizado_con = @priorizado_con,
           comentario_priorizacion = @comentario_priorizacion,
           estado = CASE WHEN @yaPriorizada = 1 THEN 'Repriorizada' ELSE 'Priorizada' END,
           fecha_actualizacion = GETDATE(),
           actualizado_por = @idusuario
     WHERE idsolicitud = @idsolicitud;

    INSERT INTO dbo.Bitacora (entidad, identidad, idusuario, campo, valor_anterior, valor_nuevo, tipo_evento, motivo)
    VALUES (
        'Solicitud', @idsolicitud, @idusuario, 'prioridad', @prioridad_anterior, @prioridad,
        CASE WHEN @yaPriorizada = 1 THEN 'Repriorizacion' ELSE 'Priorizacion' END,
        @motivo
    );
END
GO

-- Cambia una fecha comprometida ya existente; motivo obligatorio (validado en API).
CREATE OR ALTER PROCEDURE dbo.sp_Solicitudes_CambiarFechaComprometida
    @idsolicitud INT,
    @fecha_nueva DATE,
    @idusuario   INT,
    @motivo      VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @fecha_anterior DATE;
    SELECT @fecha_anterior = fecha_comprometida FROM dbo.Solicitudes WHERE idsolicitud = @idsolicitud;

    UPDATE dbo.Solicitudes
       SET fecha_comprometida = @fecha_nueva,
           fecha_actualizacion = GETDATE(),
           actualizado_por = @idusuario
     WHERE idsolicitud = @idsolicitud;

    INSERT INTO dbo.Bitacora (entidad, identidad, idusuario, campo, valor_anterior, valor_nuevo, tipo_evento, motivo)
    VALUES ('Solicitud', @idsolicitud, @idusuario, 'fecha_comprometida',
            CONVERT(VARCHAR(10), @fecha_anterior, 120), CONVERT(VARCHAR(10), @fecha_nueva, 120),
            'CambioFecha', @motivo);
END
GO

/* ============================================================================
   ACTIVIDADES
   ============================================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Actividades_Crear
    @idsolicitud         INT,
    @descripcion         VARCHAR(500),
    @estimacion_horas    DECIMAL(6,2),
    @idusuario_developer INT,
    @creado_por          INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Actividades (idsolicitud, descripcion, estimacion_horas, idusuario_developer, creado_por)
    VALUES (@idsolicitud, @descripcion, @estimacion_horas, @idusuario_developer, @creado_por);

    DECLARE @idactividad INT = SCOPE_IDENTITY();

    INSERT INTO dbo.Bitacora (entidad, identidad, idusuario, campo, valor_anterior, valor_nuevo, tipo_evento)
    VALUES ('Actividad', @idactividad, @creado_por, 'creacion', NULL, @descripcion, 'Creacion');

    -- Si la solicitud aún no tenía developer asignado, la marcamos como 'Asignada'
    UPDATE dbo.Solicitudes
       SET estado = 'Asignada', fecha_actualizacion = GETDATE()
     WHERE idsolicitud = @idsolicitud
       AND estado IN ('Priorizada','Repriorizada');

    SELECT @idactividad AS idactividad;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Actividades_Listado
    @idsolicitud INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idactividad, idsolicitud, descripcion, estimacion_horas, idusuario_developer,
           estado, fecha_inicio_real, fecha_fin_real, fecha_creacion
    FROM dbo.Actividades
    WHERE idsolicitud = @idsolicitud
    ORDER BY idactividad;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Actividades_ListadoPorDeveloper
    @idusuario_developer INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.idactividad, a.idsolicitud,
           'TI-' + RIGHT('000000' + CAST(a.idsolicitud AS VARCHAR(6)), 6) AS folio,
           s.titulo AS titulo_solicitud,
           a.descripcion, a.estimacion_horas, a.estado,
           a.fecha_inicio_real, a.fecha_fin_real
    FROM dbo.Actividades a
    INNER JOIN dbo.Solicitudes s ON s.idsolicitud = a.idsolicitud
    WHERE a.idusuario_developer = @idusuario_developer
    ORDER BY a.estado, a.idactividad DESC;
END
GO

-- El API debe validar que @idusuario sea el developer dueño de la actividad
-- (o Admin) ANTES de llamar este SP.
CREATE OR ALTER PROCEDURE dbo.sp_Actividades_MarcarEstado
    @idactividad INT,
    @estado      VARCHAR(20),   -- 'Pendiente' | 'En progreso' | 'Terminada'
    @idusuario   INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @estado_anterior VARCHAR(20), @idsolicitud INT;
    SELECT @estado_anterior = estado, @idsolicitud = idsolicitud
    FROM dbo.Actividades WHERE idactividad = @idactividad;

    UPDATE dbo.Actividades
       SET estado = @estado,
           fecha_inicio_real = CASE WHEN @estado = 'En progreso' AND fecha_inicio_real IS NULL THEN GETDATE() ELSE fecha_inicio_real END,
           fecha_fin_real    = CASE WHEN @estado = 'Terminada' THEN GETDATE() ELSE fecha_fin_real END
     WHERE idactividad = @idactividad;

    INSERT INTO dbo.Bitacora (entidad, identidad, idusuario, campo, valor_anterior, valor_nuevo, tipo_evento)
    VALUES ('Actividad', @idactividad, @idusuario, 'estado', @estado_anterior, @estado, 'CambioEstado');

    -- Si todas las actividades de la solicitud quedaron terminadas, la solicitud pasa a 'En pruebas'
    IF @estado = 'Terminada' AND NOT EXISTS (
        SELECT 1 FROM dbo.Actividades WHERE idsolicitud = @idsolicitud AND estado <> 'Terminada'
    )
    BEGIN
        UPDATE dbo.Solicitudes
           SET estado = 'En pruebas', fecha_actualizacion = GETDATE()
         WHERE idsolicitud = @idsolicitud AND estado NOT IN ('En pruebas','Completada','Cancelada');
    END
    ELSE IF @estado = 'En progreso'
    BEGIN
        UPDATE dbo.Solicitudes
           SET estado = 'En progreso', fecha_actualizacion = GETDATE()
         WHERE idsolicitud = @idsolicitud AND estado = 'Asignada';
    END
END
GO

/* ============================================================================
   BITACORA
   ============================================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Bitacora_Historial
    @entidad   VARCHAR(20),
    @identidad INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idbitacora, entidad, identidad, idusuario, campo, valor_anterior, valor_nuevo,
           tipo_evento, motivo, fecha_hora
    FROM dbo.Bitacora
    WHERE entidad = @entidad AND identidad = @identidad
    ORDER BY fecha_hora DESC;
END
GO
