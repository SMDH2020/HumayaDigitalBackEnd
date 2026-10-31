using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Roles
{
    // Inverso de AD_TI_Roles_Mio: dado un codigo de rol (ej. "JEFATURA_TI"),
    // regresa los usuarios activos con ese rol activo, con nombre/correo ya
    // resueltos. Usado para armar destinatarios de notificaciones por
    // correo del modulo.
    public class AD_TI_Roles_UsuariosPorRol
    {
        private string CadenaConexion;
        public AD_TI_Roles_UsuariosPorRol(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<IEnumerable<mdl_TI_UsuarioContacto>> Listado(string rolCodigo)
        {
            try
            {
                var parametros = new { RolCodigo = rolCodigo };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_TI_UsuarioContacto> result = await factory.SQL.QueryAsync<mdl_TI_UsuarioContacto>(
                    "dbo.sp_TI_UsuarioRoles_ListadoPorRol", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
