using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.SolicitudCredito
{
    public class AD_Usuarios_Rol_Listado
    {
        private string CadenaConexion;
        public AD_Usuarios_Rol_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlUsuariosRol>> Listado(string usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlUsuariosRol> result = await factory.SQL.QueryAsync<mdlUsuariosRol>("dbo.sp_Roles_Usuarios_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
