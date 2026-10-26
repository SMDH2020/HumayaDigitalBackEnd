namespace HD_GestionProyectosTI.Modelos
{
    // Fila de listado/detalle tal como la regresan sp_Solicitudes_Listado y
    // sp_Solicitudes_Obtener (join contra TI_TiposSolicitud, TI_EstadosSolicitud
    // y TI_Prioridades para traer codigo+nombre legibles).
    public class mdl_TI_Solicitud
    {
        public long SolicitudId { get; set; }
        public string Folio { get; set; } = "";
        public string Titulo { get; set; } = "";
        public string? DescripcionNecesidad { get; set; }
        public string? ProblemaActual { get; set; }
        public string? BeneficioEsperado { get; set; }

        public int TipoSolicitudId { get; set; }
        public string? TipoSolicitudCodigo { get; set; }
        public string? TipoSolicitud { get; set; }

        public int EstadoSolicitudId { get; set; }
        public string? EstadoCodigo { get; set; }
        public string? EstadoSolicitud { get; set; }
        public string? Etapa { get; set; }

        public int? PrioridadId { get; set; }
        public string? PrioridadCodigo { get; set; }
        public string? Prioridad { get; set; }

        public int SolicitanteUsuarioId { get; set; }
        public int CreadaPorUsuarioId { get; set; }
        public string? SolicitanteNombre { get; set; }
        public string? CreadaPorNombre { get; set; }
        public string? SolicitanteCorreo { get; set; }
        public string? AreaSolicitante { get; set; }
        public bool EsConfidencial { get; set; }

        public string? MotivoRechazo { get; set; }
        public string? MotivoCancelacion { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime FechaUltimaActualizacion { get; set; }
        public DateTime? FechaCierre { get; set; }

        // Avance por funcionalidades de la version de alcance APROBADA
        // vigente (ver 20_ListadoBandejaConAvance.sql); NULL mientras la
        // solicitud no tenga ninguna version de alcance aprobada todavia.
        public decimal? TotalDiasEstimados { get; set; }
        public int? FuncionalidadesTotal { get; set; }
        public int? FuncionalidadesAprobadas { get; set; }

        // Avance REAL ponderado por dias (ver 31_FixAvanceBandejaNoEsPriorizacion.sql):
        // Sum(DiasEstimados * AvanceManual) de las funcionalidades activas de
        // la version vigente -- el frontend lo divide entre TotalDiasEstimados
        // para obtener el %. Antes la columna "Avance" de la Bandeja usaba
        // FuncionalidadesAprobadas/FuncionalidadesTotal, que desde
        // 26_FixFuncionalidadesPriorizadasDefinicion.sql ya no mide avance
        // real sino solo priorizacion (mes/anio asignado) -- una solicitud
        // con todo programado pero sin trabajo real mostraba 100%.
        public decimal? SumaDiasPorAvance { get; set; }

        // Meses priorizados (columna "Priorizado" de la Bandeja): pares
        // "anio-mes" separados por coma de las funcionalidades vigentes que
        // ya tienen MesProgramado/AnioProgramado, ordenados cronologicamente
        // (ver 23_MesesPriorizadosBandeja.sql). NULL/vacio = ninguna
        // funcionalidad priorizada todavia -- el frontend lo muestra como
        // "Pendiente de priorizar".
        public string? MesesPriorizados { get; set; }
    }
}
