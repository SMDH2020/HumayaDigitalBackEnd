-- Eliminar una solicitud por completo (sin importar su estatus), a pedido
-- explicito de Jefatura de TI -- unico rol habilitado (ver
-- SolicitudesTIController.EsJefaturaTIAsync). No es un soft-delete: borra la
-- solicitud y todo lo que cuelga de ella. El front exige escribir el folio
-- exacto como confirmacion antes de llamar a este endpoint; aqui solo se
-- revalida que la solicitud exista.
--
-- Orden de borrado = mismo criterio que 99_LimpiarDatosPrueba... (hijos
-- antes que padres), pero acotado por SolicitudId en vez de vaciar toda la
-- tabla. Incluye dbo.TI_AprobacionesAlcance y dbo.TI_FuncionalidadDesarrolladores
-- (a diferencia del script 99, aqui si se asume que existen: ambas estan en
-- uso activo hoy -- VersionesAlcanceTI/Autorizar y BacklogTI/AsignarDesarrolladores).
CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Eliminar
    @SolicitudId BIGINT,
    @UsuarioId   INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.TI_Solicitudes WHERE SolicitudId = @SolicitudId)
    BEGIN
        RAISERROR('La solicitud indicada no existe', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION;

        DELETE FROM dbo.TI_AuditoriaEventos WHERE SolicitudId = @SolicitudId;
        DELETE FROM dbo.TI_CambiosProgramacion WHERE SolicitudId = @SolicitudId;
        DELETE FROM dbo.TI_SolicitudEstadosHistorial WHERE SolicitudId = @SolicitudId;
        DELETE FROM dbo.TI_SolicitudParticipantes WHERE SolicitudId = @SolicitudId;

        DELETE fd
        FROM dbo.TI_FuncionalidadDesarrolladores fd
        INNER JOIN dbo.TI_Funcionalidades f ON f.FuncionalidadId = fd.FuncionalidadId
        INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
        WHERE v.SolicitudId = @SolicitudId;

        DELETE f
        FROM dbo.TI_Funcionalidades f
        INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
        WHERE v.SolicitudId = @SolicitudId;

        IF OBJECT_ID('dbo.TI_AprobacionesAlcance', 'U') IS NOT NULL
            DELETE a
            FROM dbo.TI_AprobacionesAlcance a
            INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = a.VersionAlcanceId
            WHERE v.SolicitudId = @SolicitudId;

        DELETE FROM dbo.TI_VersionesAlcance WHERE SolicitudId = @SolicitudId;
        DELETE FROM dbo.TI_Solicitudes WHERE SolicitudId = @SolicitudId;

    COMMIT TRANSACTION;
END
