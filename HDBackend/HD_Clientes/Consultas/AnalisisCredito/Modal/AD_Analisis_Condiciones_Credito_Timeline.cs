using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito.Modal
{
    public class AD_Analisis_Condiciones_Credito_Timeline
    {
        private string CadenaConexion;
        public AD_Analisis_Condiciones_Credito_Timeline(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlPedido_Condiciones_Venta> Condiciones(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                mdlPedido_Condiciones_Venta result = await factory.SQL.QueryFirstOrDefaultAsync<mdlPedido_Condiciones_Venta>("Credito.sp_Analisis_Condiciones_Credito_Timeline", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
