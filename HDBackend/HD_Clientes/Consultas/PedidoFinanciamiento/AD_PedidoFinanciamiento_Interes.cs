using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.PedidoFinanciamiento
{
    public class AD_PedidoFinanciamiento_Interes
    {

        private string CadenaConexion;
        public AD_PedidoFinanciamiento_Interes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlPedido_Condiciones_Venta> Get(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                mdlPedido_Condiciones_Venta result = await factory.SQL.QueryFirstOrDefaultAsync<mdlPedido_Condiciones_Venta>("Credito.sp_Pedido_Detalle_Financiamiento_Interes", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
