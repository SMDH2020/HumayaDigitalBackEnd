public class mdlSeguimiento_Email
{
    public int idSala { get; set; }
    public int idSolicitud { get; set; }
    public string? folio { get; set; }
    public string? nombreSala { get; set; }
    public string? actividad { get; set; }
    public string? comentarios { get; set; }
    public string? estatus { get; set; }

    // Nombre de quien creó el ticket (compatibilidad con el diseño previo).
    public string? usuario { get; set; }

    // Nombre de quien ejecutó la acción que dispara este correo en
    // particular (puede ser el mismo creador, o el responsable que atendió
    // el ticket) -- es lo que se muestra en la tarjeta del correo.
    public string? accionPor { get; set; }
}