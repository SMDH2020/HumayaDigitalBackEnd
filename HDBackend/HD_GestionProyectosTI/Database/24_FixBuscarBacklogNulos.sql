-- ─────────────────────────────────────────────────────────────────────────
-- Fix sp_TI_Backlog_Buscar: dos comparaciones del WHERE quedaban en NULL
-- (desconocido) en vez de TRUE/FALSE cuando f.MesProgramado/AnioProgramado
-- o uc.EstadoCambio eran NULL -- y SQL Server descarta cualquier fila cuyo
-- WHERE evalue a NULL, no solo a FALSE. Eso excluia casi todo:
--
--   1) Funcionalidades sin priorizar (MesProgramado/AnioProgramado NULL):
--      "NOT (NULL = @Mes AND NULL = @Anio)" = NOT NULL = NULL -> excluida.
--      Por eso el modal "Buscar" nunca mostraba las pendientes de priorizar.
--
--   2) Funcionalidades sin ningun TI_CambiosProgramacion (la mayoria): el
--      LEFT JOIN deja uc.EstadoCambio en NULL, y
--      "NOT (uc.EstadoCambio = 'APROBADO' AND ...)" tambien da NULL -> se
--      excluian casi todas las filas sin importar la busqueda, lo que hacia
--      parecer que el buscador "no funcionaba" (resultados vacios o casi
--      vacios sin importar el texto).
-- ─────────────────────────────────────────────────────────────────────────

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
      AND (f.AvanceManual IS NULL OR f.AvanceManual < 100)
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
