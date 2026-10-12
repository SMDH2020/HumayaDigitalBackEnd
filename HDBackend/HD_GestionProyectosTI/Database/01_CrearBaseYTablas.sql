/* ============================================================================
   HumayaDigital_GestionProyectosTI
   Base de datos dedicada al proceso de solicitud, evaluación, priorización
   y seguimiento de proyectos de TI.

   Motor: SQL Server. Debe crearse en el MISMO servidor que humayadigital_usuarios,
   para poder referenciar en tres partes su catálogo de módulos existente
   (humayadigital_usuarios.dbo.sp_modulos_dropdownlist), tal como ya se hace
   en HD_Generales/AD_Modulos_Dropdownlist.cs.
   ============================================================================ */

IF DB_ID('HumayaDigital_GestionProyectosTI') IS NULL
BEGIN
    CREATE DATABASE HumayaDigital_GestionProyectosTI;
END
GO

USE HumayaDigital_GestionProyectosTI;
GO

/* ----------------------------------------------------------------------------
   RolesTI
   Rol de cada usuario dentro de ESTE proceso. No toca el catálogo de roles
   general del sistema (ese controla menús/módulos de negocio, es otra cosa).
   Si un idusuario no tiene fila aquí, la aplicación lo trata como 'Usuario'
   (cualquier empleado puede solicitar). Solo se registra explícitamente a
   quien es Developer o Admin de este proceso.
   ---------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.RolesTI', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.RolesTI (
        idusuario       INT             NOT NULL PRIMARY KEY,
        rol             VARCHAR(20)     NOT NULL CHECK (rol IN ('Developer','Admin')),
        fechaAsignacion DATETIME        NOT NULL DEFAULT GETDATE(),
        asignadoPor     INT             NULL
    );
END
GO

/* ----------------------------------------------------------------------------
   Solicitudes
   Tabla principal: incidencias, nuevos desarrollos y mejoras.
   idmodulo e idusuario_* son referencias "suaves" (sin FK físico) al
   catálogo de usuarios/módulos que vive en humayadigital_usuarios, porque
   está en otra base de datos.
   ---------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Solicitudes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Solicitudes (
        idsolicitud                     INT IDENTITY(1,1) PRIMARY KEY,

        tipo                             VARCHAR(20)   NOT NULL
            CHECK (tipo IN ('Incidencia','Nuevo desarrollo','Mejora')),
        titulo                           VARCHAR(200)  NOT NULL,
        descripcion                      VARCHAR(MAX)  NOT NULL,
        idmodulo                         INT           NOT NULL,

        idusuario_solicitante            INT           NOT NULL,

        -- Las 5 preguntas del formulario actual
        pregunta_informacion             VARCHAR(MAX)  NOT NULL,
        pregunta_objetivo_negocio        VARCHAR(MAX)  NOT NULL,
        pregunta_decisiones              VARCHAR(MAX)  NOT NULL,
        pregunta_frecuencia              VARCHAR(200)  NOT NULL,
        pregunta_uso_compartido          VARCHAR(MAX)  NOT NULL,

        -- Impacto de negocio (solo Nuevo desarrollo / Mejora, validado en app)
        impacto_control_interno          BIT           NOT NULL DEFAULT 0,
        impacto_normativo                BIT           NOT NULL DEFAULT 0,
        impacto_financiero                BIT           NOT NULL DEFAULT 0,
        impacto_comentario               VARCHAR(MAX)  NULL,

        estado                           VARCHAR(40)   NOT NULL DEFAULT 'Nueva'
            CHECK (estado IN (
                'Nueva','En revisión','Rechazada','Aceptada',
                'En definición','Pendiente de aprobación del usuario',
                'Priorizada','Repriorizada','Asignada','En progreso',
                'En pruebas','Completada','Cancelada'
            )),

        prioridad                        VARCHAR(10)   NULL
            CHECK (prioridad IN ('Crítica','Alta','Media','Baja')),
        priorizado_con                   VARCHAR(200)  NULL,
        comentario_priorizacion          VARCHAR(MAX)  NULL,

        rondas_rechazo_alcance           INT           NOT NULL DEFAULT 0,

        fecha_estimada                   DATE          NULL,
        fecha_comprometida               DATE          NULL,

        fecha_creacion                   DATETIME      NOT NULL DEFAULT GETDATE(),
        fecha_actualizacion              DATETIME      NOT NULL DEFAULT GETDATE(),
        creado_por                       INT           NOT NULL,
        actualizado_por                  INT           NULL
    );

    CREATE INDEX IX_Solicitudes_Solicitante ON dbo.Solicitudes(idusuario_solicitante);
    CREATE INDEX IX_Solicitudes_Estado      ON dbo.Solicitudes(estado);
    CREATE INDEX IX_Solicitudes_Modulo      ON dbo.Solicitudes(idmodulo);
END
GO

/* ----------------------------------------------------------------------------
   Actividades
   Desglose de trabajo de una solicitud (nuevo desarrollo / mejora, y
   opcionalmente incidencias grandes). El developer solo edita el estado
   de sus propias actividades.
   ---------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Actividades', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Actividades (
        idactividad         INT IDENTITY(1,1) PRIMARY KEY,
        idsolicitud          INT           NOT NULL
            FOREIGN KEY REFERENCES dbo.Solicitudes(idsolicitud),
        descripcion          VARCHAR(500)  NOT NULL,
        estimacion_horas     DECIMAL(6,2)  NOT NULL,
        idusuario_developer  INT           NOT NULL,
        estado               VARCHAR(20)   NOT NULL DEFAULT 'Pendiente'
            CHECK (estado IN ('Pendiente','En progreso','Terminada')),
        fecha_inicio_real    DATETIME      NULL,
        fecha_fin_real       DATETIME      NULL,
        fecha_creacion       DATETIME      NOT NULL DEFAULT GETDATE(),
        creado_por           INT           NOT NULL
    );

    CREATE INDEX IX_Actividades_Solicitud ON dbo.Actividades(idsolicitud);
    CREATE INDEX IX_Actividades_Developer ON dbo.Actividades(idusuario_developer);
END
GO

/* ----------------------------------------------------------------------------
   Bitacora
   Bitácora única e inmutable de cambios (no se llama "Auditoria" porque ese
   nombre ya lo usa el módulo de auditoría de inventario físico). El motivo
   es obligatorio, a nivel aplicación, solo para ciertos tipo_evento:
   CambioFecha, RechazoAlcance, Repriorizacion, Cancelacion.
   ---------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Bitacora', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Bitacora (
        idbitacora     INT IDENTITY(1,1) PRIMARY KEY,
        entidad        VARCHAR(20)   NOT NULL CHECK (entidad IN ('Solicitud','Actividad')),
        identidad      INT           NOT NULL,
        idusuario      INT           NOT NULL,
        campo          VARCHAR(100)  NOT NULL,
        valor_anterior VARCHAR(MAX)  NULL,
        valor_nuevo    VARCHAR(MAX)  NULL,
        tipo_evento    VARCHAR(40)   NOT NULL,
        motivo         VARCHAR(MAX)  NULL,
        fecha_hora     DATETIME      NOT NULL DEFAULT GETDATE()
    );

    CREATE INDEX IX_Bitacora_Entidad ON dbo.Bitacora(entidad, identidad);
END
GO
