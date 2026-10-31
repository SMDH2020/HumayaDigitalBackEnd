using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoFinanciamiento
{
    public class AD_PedidoFinanciamiento_Docto
    {
        private string CadenaConexion;
        public AD_PedidoFinanciamiento_Docto(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlPedido_Detalle_Financiamiento> Get(string folio, int docto)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    docto
                };
                mdlPedido_Detalle_Financiamiento result = await factory.SQL.QueryFirstOrDefaultAsync<mdlPedido_Detalle_Financiamiento>("Credito.sp_Pedido_Detalle_Financiamiento_ByDocto", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
