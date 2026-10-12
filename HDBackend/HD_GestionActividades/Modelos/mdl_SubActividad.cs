namespace HD_GestionActividades.Modelos
{
    // Cat_SubActividad: catálogo (plantilla) del checklist que puede tener
    // una actividad. Cuando se crea un ticket para esa actividad, cada fila
    // activa se clona hacia Cat_SeguimientoAct_SubActividad (ver
    // mdl_SeguimientoAct_SubActividad) -- este modelo es solo el catálogo,
    // no lleva estado de "completado".
    public class mdl_SubActividad
    {
        public int idSubActividad { get; set; }
        public int idActividad { get; set; }
        public string? nombreSubActividad { get; set; }
        public int orden { get; set; }
        public bool estado { get; set; }
        public int usuario { get; set; }
    }
}
