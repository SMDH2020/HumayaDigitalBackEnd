using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoCondicionesCredito
{
    public class AD_PedidoCondicionesVenta_Listado
    {
        private string CadenaConexion;
        public AD_PedidoCondicionesVenta_Listado(string _cadenaconexion)
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
                mdlPedido_Condiciones_Venta result = await factory.SQL.QueryFirstOrDefaultAsync<mdlPedido_Condiciones_Venta>("Credito.sp_Pedido_Condiciones_Venta_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) result = new mdlPedido_Condiciones_Venta();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdlCondiciones_Venta_View> Obtener(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Pedido_Condiciones_Credito_Venta_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlCondiciones_Venta_View view = new mdlCondiciones_Venta_View();
                view.condiciones = result.Read<mdlPedido_Condiciones_Venta>().FirstOrDefault();
                view.interes = result.Read<mdlInteres_Credito>().FirstOrDefault();
                factory.SQL.Close();
                //if (view.mdlSolicitud == null) view.mdlSolicitud = new mdlSolicitudCredito_Enviar();
                if (view.condiciones == null) view.condiciones = new mdlPedido_Condiciones_Venta();
                if (view.interes == null) view.interes = new mdlInteres_Credito();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
