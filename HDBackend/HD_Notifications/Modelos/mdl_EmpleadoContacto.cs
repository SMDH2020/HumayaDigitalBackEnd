// Datos mínimos de contacto de un empleado (nombre + correo), usados por
// las notificaciones de Seguimiento de Actividades para saber a quién
// dirigir cada correo (creador del ticket, responsable de la sala, etc.)
// sin tener que repetir el mismo query en cada lugar.
public class mdl_EmpleadoContacto
{
    public string? Nombre { get; set; }
    public string? Correo { get; set; }
}
