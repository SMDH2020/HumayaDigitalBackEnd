using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoDatosGenerales
{
    public class AD_PedidosGenerales_GetByFolio
    {
        private string CadenaConexion;
        public AD_PedidosGenerales_GetByFolio(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlPedido_Datos_Generales> Get(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                mdlPedido_Datos_Generales result = await factory.SQL.QueryFirstOrDefaultAsync<mdlPedido_Datos_Generales>("Credito.sp_Pedido_Datos_Solicitante", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if(result == null) { result = new mdlPedido_Datos_Generales();
                    result.fechaentrega = DateTime.Now;
                }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
