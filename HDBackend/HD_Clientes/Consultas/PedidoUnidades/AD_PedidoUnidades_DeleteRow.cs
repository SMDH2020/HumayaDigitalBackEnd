using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoUnidades
{
    public class AD_PedidoUnidades_DeleteRow
    {
        private string CadenaConexion;
        public AD_PedidoUnidades_DeleteRow(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlPedido_Unidades>> Delete(string folio,int registro,string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    registro,
                    usuario
                };
                IEnumerable<mdlPedido_Unidades> result = await factory.SQL.QueryAsync<mdlPedido_Unidades>("Credito.sp_Pedido_Unidades_DeleteRow", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
