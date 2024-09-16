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
    public class AD_RelMenuModulo_Listado
    {
        private string CadenaConexion;
        public AD_RelMenuModulo_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlRelMenuModulo>> ListadoMenu(int idmodulo)
        {
            try
            {
                var parametros = new
                {
                    idmodulo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlRelMenuModulo> result = await factory.SQL.QueryAsync<mdlRelMenuModulo>("humayadigital_usuarios.dbo.sp_Usuarios_Rel_Menu_Modulo_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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

