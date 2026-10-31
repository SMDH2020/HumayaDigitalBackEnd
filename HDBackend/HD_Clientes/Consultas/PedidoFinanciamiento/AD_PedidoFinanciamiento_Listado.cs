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
        public async Task<mdlPedido_Detalle_Financiamiento_View> Get(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Pedido_Detalle_Financiamiento_info", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlPedido_Detalle_Financiamiento_View view = new mdlPedido_Detalle_Financiamiento_View();
                view.info = result.Read<mdlPedido_Detalle_Financiamiento_Info>().FirstOrDefault();
                view.detalle_financiamiento = result.Read<mdlPedido_Detalle_Financiamiento>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
