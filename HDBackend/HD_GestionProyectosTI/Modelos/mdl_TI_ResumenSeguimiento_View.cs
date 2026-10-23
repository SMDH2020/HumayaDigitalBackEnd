namespace HD_GestionProyectosTI.Modelos
{
    // Agrupa los 3 result sets de sp_TI_Solicitudes_ResumenSeguimiento
    // (QueryMultipleAsync): el resumen de una fila y las 2 listas de
    // aprobaciones (alcance vigente / decision de prioridad vigente).
    public class mdl_TI_ResumenSeguimiento_View
    {
        public long SolicitudId { get; set; }

        public long? VersionAlcanceId { get; set; }
        public int? VersionAlcanceNumero { get; set; }
        public string? EstadoVersion { get; set; }

        public long? EstimacionId { get; set; }
        public int? EstimacionNumero { get; set; }
        public string? EstadoEstimacion { get; set; }
        public decimal? HorasTotal { get; set; }

        public long? EvaluacionPrioridadId { get; set; }
        public int? NumeroEvaluacion { get; set; }
        public string? EstadoEvaluacion { get; set; }
        public decimal? PuntajeTotal { get; set; }

        public long? DecisionPrioridadId { get; set; }
        public int? NumeroDecision { get; set; }
        public string? TipoDecision { get; set; }
        public string? EstadoDecision { get; set; }
        public int? DecisionPrioridadIdSeleccionada { get; set; }
        public int? PosicionBacklog { get; set; }

        // Repriorizacion PENDIENTE de autorizacion de Planeacion (si existe):
        // la decision/programacion vigente arriba NO se toca hasta que se
        // resuelva. A lo sumo una a la vez (reforzado en
        // sp_TI_DecisionesPrioridad_Crear).
        public long? RepriorizacionPendienteId { get; set; }
        public int? RepriorizacionNumeroDecision { get; set; }
        public string? RepriorizacionTipoDecision { get; set; }
        public int? RepriorizacionPrioridadId { get; set; }
        public int? RepriorizacionPosicionBacklog { get; set; }
        public string? RepriorizacionMotivo { get; set; }

        public IEnumerable<mdl_TI_AprobacionResumen> AprobacionesAlcance { get; set; } = new List<mdl_TI_AprobacionResumen>();
        public IEnumerable<mdl_TI_AprobacionResumen> AprobacionesPrioridad { get; set; } = new List<mdl_TI_AprobacionResumen>();
        public IEnumerable<mdl_TI_AprobacionResumen> AprobacionesRepriorizacion { get; set; } = new List<mdl_TI_AprobacionResumen>();
    }
}
