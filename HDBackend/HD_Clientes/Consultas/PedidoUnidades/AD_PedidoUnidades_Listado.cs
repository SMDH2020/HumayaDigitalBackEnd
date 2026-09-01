using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoUnidades
{
    public class AD_PedidoUnidades_Listado
    {
        private string CadenaConexion;
        public AD_PedidoUnidades_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlPedido_Unidades>> Get(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                IEnumerable<mdlPedido_Unidades> result = await factory.SQL.QueryAsync<mdlPedido_Unidades>("Credito.sp_Pedido_Unidades_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
