using System;

namespace HD_GestionActividades.Modelos
{
    // Instancia clonada del checklist de una actividad, ligada a UN ticket
    // en particular (Cat_SeguimientoAct_SubActividad). Se crea una copia al
    // dar de alta el ticket -- si luego cambia el catálogo (Cat_SubActividad)
    // los tickets ya creados no se ven afectados.
    public class mdl_SeguimientoAct_SubActividad
    {
        public int idSegActSubActividad { get; set; }
        public int idSolicitud { get; set; }
        public string? nombreSubActividad { get; set; }
        public int orden { get; set; }
        public bool completado { get; set; }
        public DateTime? fechaCompletado { get; set; }
        public int? usuarioCompleto { get; set; }
        public string? usuarioCompletoNombre { get; set; }
    }
}
