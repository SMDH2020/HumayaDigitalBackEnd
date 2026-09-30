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
    public class AD_Modulos_Listado
    {
        private string CadenaConexion;
        public AD_Modulos_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlModulo>> ListadoModulos()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlModulo> result = await factory.SQL.QueryAsync<mdlModulo>("humayadigital_usuarios.dbo.sp_Usuarios_Modulos_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
