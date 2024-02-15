using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoUnidades
{
    public class AD_PedidoUnidades_ByRegistro
    {
        private string CadenaConexion;
        public AD_PedidoUnidades_ByRegistro(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlPedido_Unidades> Get(string folio,int registro)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    registro
                };
                mdlPedido_Unidades result = await factory.SQL.QueryFirstOrDefaultAsync<mdlPedido_Unidades>("Credito.sp_Pedido_Unidades_GetByRegistro", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
