using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    // Input: una funcionalidad dentro del detalle enviado al crear una
    // version de alcance (va serializado como @json al SP).
    public class mdl_TI_FuncionalidadCrear
    {
        public string? Codigo { get; set; }

        [Required(ErrorMessage = "El nombre de la funcionalidad es un valor requerido")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "La descripcion de la funcionalidad es un valor requerido")]
        public string Descripcion { get; set; } = "";

        public string? ReglasNegocio { get; set; }

        [Required(ErrorMessage = "Los criterios de aceptacion son un valor requerido")]
        public string CriteriosAceptacion { get; set; } = "";

        public byte PrioridadInterna { get; set; } = 3;
        public string? Complejidad { get; set; }
        public int Orden { get; set; }

        // Dias estimados para esta funcionalidad, parte de la definicion de
        // alcance (no de una estimacion independiente). Nullable: puede
        // enviarse sin capturar aun, pero sp_TI_VersionesAlcance_EnviarAprobacion
        // exige un valor valido (no NULL, no negativo) en todas las
        // funcionalidades activas antes de permitir el envio a aprobacion.
        public decimal? DiasEstimados { get; set; }
    }

    // Output: tal como la regresa sp_TI_VersionesAlcance_Obtener.
    public class mdl_TI_Funcionalidad
    {
        public long FuncionalidadId { get; set; }
        public long VersionAlcanceId { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string? ReglasNegocio { get; set; }
        public string CriteriosAceptacion { get; set; } = "";
        public byte PrioridadInterna { get; set; }
        public string? Complejidad { get; set; }
        public int Orden { get; set; }
        public decimal? DiasEstimados { get; set; }
        public bool Activa { get; set; }
        public int CreadaPorUsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
