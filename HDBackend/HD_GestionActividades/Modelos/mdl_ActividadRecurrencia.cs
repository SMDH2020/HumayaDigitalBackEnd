namespace HD_GestionActividades.Modelos
{
    // Configuración de recurrencia de una actividad (columnas nuevas en
    // Cat_Actividad) -- usada por la creación automática de tickets
    // (SP_Cat_SeguimientoAct_CrearAutomaticos). Modelo aparte de mdl_Actividad
    // a propósito, para no tocarlo ni al flujo de Guardar/Editar existente.
    public class mdl_ActividadRecurrencia
    {
        public int idActividad { get; set; }
        public bool esRecurrente { get; set; }
        public int? idSalaRecurrente { get; set; }
        public int? idUsuarioRecurrente { get; set; }
        public string? frecuenciaRecurrente { get; set; } // 'M' Mensual / 'S' Semanal
        public int? diaRecurrente { get; set; }
        public int usuario { get; set; }
    }

    // Fila liviana para pintar el badge "RECURRENTE" en el listado del
    // catálogo sin tener que tocar SP_Cat_Actividad_Listado.
    public class mdl_ActividadRecurrenciaResumen
    {
        public int idActividad { get; set; }
        public bool esRecurrente { get; set; }
    }
}
