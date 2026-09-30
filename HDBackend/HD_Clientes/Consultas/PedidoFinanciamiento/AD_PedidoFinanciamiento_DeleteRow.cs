using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoFinanciamiento
{
    public class AD_PedidoFinanciamiento_DeleteRow
    {
        private string CadenaConexion;
        public AD_PedidoFinanciamiento_DeleteRow(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlPedido_Detalle_Financiamiento_View> Delete(string folio, int docto, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    docto,
                    usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Pedido_Detalle_Financiamiento_Delete", parametros, commandType: System.Data.CommandType.StoredProcedure);
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

        public async Task<mdlPedido_Detalle_Financiamiento_View> DeleteAll(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.Pedido_Detalle_Financiamiento_EliminarAmortizaciones", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
