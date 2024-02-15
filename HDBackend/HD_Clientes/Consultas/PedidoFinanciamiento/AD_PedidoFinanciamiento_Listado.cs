using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoFinanciamiento
{
    public class AD_PedidoFinanciamiento_Listado
    {
        private string CadenaConexion;
        public AD_PedidoFinanciamiento_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlPedido_Detalle_Financiamiento>> Get(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                IEnumerable<mdlPedido_Detalle_Financiamiento> result = await factory.SQL.QueryAsync<mdlPedido_Detalle_Financiamiento>("Credito.sp_Pedido_Detalle_Financiamiento_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
