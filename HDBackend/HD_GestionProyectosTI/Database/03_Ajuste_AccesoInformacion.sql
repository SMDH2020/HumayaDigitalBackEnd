/* ============================================================================
   Ajuste: se agrega el tipo 'Acceso a información' (solicitud de acceso a un
   menú/reporte existente) como un 4to tipo de solicitud, independiente de
   Incidencia / Nuevo desarrollo / Mejora.

   Las 5 preguntas del formulario original SOLO aplican a este tipo -- no a
   Incidencia/Nuevo desarrollo/Mejora como se había modelado antes. Por eso
   dejan de ser NOT NULL.

   Flujo de 'Acceso a información' (corto, sin desarrollo de por medio):
   Nueva -> En revisión -> Rechazada/Aceptada -> Completada (o Cancelada).
   No pasa por En definición, Pendiente de aprobación del usuario,
   Priorizada/Repriorizada, Asignada, En progreso ni En pruebas -- esos
   estados simplemente no se usan para este tipo, no hace falta quitarlos
   del catálogo porque siguen aplicando a los otros 3 tipos.

   Ejecutar contra HumayaDigital_GestionProyectosTI, después de
   01_CrearBaseYTablas.sql y 02_StoredProcedures_Fase1.sql.
   ============================================================================ */

USE HumayaDigital_GestionProyectosTI;
GO

-- 1) Ampliar el CHECK de tipo
DECLARE @ck sysname;
SELECT @ck = cc.name
FROM sys.check_constraints cc
INNER JOIN sys.columns col ON col.object_id = cc.parent_object_id AND col.column_id = cc.parent_column_id
WHERE cc.parent_object_id = OBJECT_ID('dbo.Solicitudes') AND col.name = 'tipo';

IF @ck IS NOT NULL
    EXEC('ALTER TABLE dbo.Solicitudes DROP CONSTRAINT ' + @ck);

ALTER TABLE dbo.Solicitudes
    ADD CONSTRAINT CK_Solicitudes_Tipo
    CHECK (tipo IN ('Incidencia','Nuevo desarrollo','Mejora','Acceso a información'));
GO

-- 2) Las 5 preguntas dejan de ser obligatorias a nivel de base de datos;
--    el API las exige solo cuando tipo = 'Acceso a información'.
ALTER TABLE dbo.Solicitudes ALTER COLUMN pregunta_informacion        VARCHAR(MAX) NULL;
ALTER TABLE dbo.Solicitudes ALTER COLUMN pregunta_objetivo_negocio   VARCHAR(MAX) NULL;
ALTER TABLE dbo.Solicitudes ALTER COLUMN pregunta_decisiones         VARCHAR(MAX) NULL;
ALTER TABLE dbo.Solicitudes ALTER COLUMN pregunta_frecuencia         VARCHAR(200) NULL;
ALTER TABLE dbo.Solicitudes ALTER COLUMN pregunta_uso_compartido     VARCHAR(MAX) NULL;
GO

-- 3) sp_Solicitudes_Crear: las 5 preguntas ahora son parámetros opcionales
--    (DEFAULT NULL), para que Incidencia/Nuevo desarrollo/Mejora puedan
--    omitirlas sin que Dapper truene por parámetro faltante.
CREATE OR ALTER PROCEDURE dbo.sp_Solicitudes_Crear
    @tipo                        VARCHAR(20),
    @titulo                      VARCHAR(200),
    @descripcion                 VARCHAR(MAX),
    @idmodulo                    INT,
    @idusuario_solicitante       INT,
    @pregunta_informacion        VARCHAR(MAX) = NULL,
    @pregunta_objetivo_negocio   VARCHAR(MAX) = NULL,
    @pregunta_decisiones         VARCHAR(MAX) = NULL,
    @pregunta_frecuencia         VARCHAR(200) = NULL,
    @pregunta_uso_compartido     VARCHAR(MAX) = NULL,
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
