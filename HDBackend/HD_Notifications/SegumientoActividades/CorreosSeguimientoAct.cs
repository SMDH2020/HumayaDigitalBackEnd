using Dapper;
using System.Data.SqlClient;

namespace HD.Notifications.SeguimientoActividades
{
    // Centraliza cómo se obtienen los correos/nombres de las personas
    // involucradas en un ticket de Seguimiento de Actividades, para no
    // repetir la misma consulta en cada clase de notificación (creación,
    // cambio de estatus, comentario).
    public static class CorreosSeguimientoAct
    {
        // Responsables activos de la sala del ticket (a quienes se avisa
        // cuando se crea un ticket nuevo, o cuando el creador comenta).
        public static List<string> ObtenerCorreosResponsables(int idSala, string conexion)
        {
            using (var con = new SqlConnection(conexion))
            {
                string query = @"
                    SELECT e.Correo
                    FROM Seguimiento_Actividades.dbo.Rel_Sala_Responsable r
                    INNER JOIN AppMH.dbo.Empleados e
                        ON r.IDEmpleado = e.IDEmpleado
                    WHERE r.idSala = @idSala
                    AND r.estado = 1
                    AND e.Estatus = 1
                ";

                return con.Query<string>(query, new { idSala })
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Distinct()
                    .ToList();
            }
        }

        // Nombre + correo de un empleado puntual (ej. quien creó el ticket
        // o quien está atendiéndolo), usado para avisarle directamente de
        // cambios de estatus o comentarios de la otra parte.
        public static mdl_EmpleadoContacto? ObtenerContactoEmpleado(int? idEmpleado, string conexion)
        {
            if (idEmpleado == null || idEmpleado == 0)
                return null;

            using (var con = new SqlConnection(conexion))
            {
                string query = @"
                    SELECT
                        e.Nombre + ' ' + e.ApellidoPaterno + ' ' + e.ApellidoMaterno AS Nombre,
                        e.Correo AS Correo
                    FROM AppMH.dbo.Empleados e
                    WHERE e.IDEmpleado = @idEmpleado
                    AND e.Estatus = 1
                ";

                return con.QueryFirstOrDefault<mdl_EmpleadoContacto>(query, new { idEmpleado });
            }
        }
    }
}
