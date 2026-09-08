namespace HD_GestionProyectosTI.Modelos
{
    // Usado para: revisar -> aceptar/rechazar, pasar a definicion, pasar a
    // pendiente de aprobacion, aprobar/rechazar alcance, cancelar.
    // motivo es obligatorio (validado en el controller) cuando estado_nuevo
    // es 'Rechazada', 'Cancelada', o cuando regresa de 'Pendiente de
    // aprobación del usuario' a 'En definición'.
    public class mdl_CambioEstado
    {
        public int idsolicitud { get; set; }
        public string estado_nuevo { get; set; } = "";
        public string? motivo { get; set; }
    }
}
