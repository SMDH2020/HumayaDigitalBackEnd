using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales.Consultas
{
    public class AD_RelUsuariosMenu_Listado
    {
        private string CadenaConexion;
        public AD_RelUsuariosMenu_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlRelUsuarioMenu>> ListadoUsuarioMenu(int idmodulo, int idusuario)
        {
            try
            {
                var parametros = new
                {
                    idmodulo,
                    idusuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlRelUsuarioMenu> result = await factory.SQL.QueryAsync<mdlRelUsuarioMenu>("humayadigital_usuarios.dbo.sp_Usuarios_Rel_Usuarios_Menu_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
