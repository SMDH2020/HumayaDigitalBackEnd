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
    public class AD_Actualizar_Interes_Condiciones
    {
        private string CadenaConexion;
        public AD_Actualizar_Interes_Condiciones(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlInteres_Credito> Get(string folio, int ejercicio, int periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    ejercicio,
                    periodo
                };
                mdlInteres_Credito result = await factory.SQL.QueryFirstOrDefaultAsync<mdlInteres_Credito>("Credito.sp_Actualizar_Interes_Condiciones", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                //if (result == null) result = new mdlInteres_Credito();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
