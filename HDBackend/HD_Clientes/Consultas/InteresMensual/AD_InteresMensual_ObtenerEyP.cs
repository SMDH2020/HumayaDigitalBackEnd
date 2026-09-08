using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.InteresMensual
{
    public class AD_InteresMensual_ObtenerEyP
    {
        private string CadenaConexion;
        public AD_InteresMensual_ObtenerEyP(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlInteres_Mensual> BuscarEyP(int ejercicio, int periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio,
                    periodo
                };
                mdlInteres_Mensual result = await factory.SQL.QueryFirstOrDefaultAsync<mdlInteres_Mensual>("Credito.Interes_Mensual_ObtenerEyP", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
