using Dapper;
using HD.AccesoDatos;
using HD.Generales.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales.Consultas
{
    public class AD_Comparativa_Presas_Listado
    {
        private string CadenaConexion;
        public AD_Comparativa_Presas_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Presas_Listado>> listadoPresas()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Presas_Listado> result = await factory.SQL.QueryAsync<mdl_Presas_Listado>("humayadigital_usuarios.dbo.sp_obtener_Comparativa_Presas_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
