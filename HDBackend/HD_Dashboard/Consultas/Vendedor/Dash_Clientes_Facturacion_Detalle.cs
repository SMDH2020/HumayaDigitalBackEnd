using Dapper;
using HD.AccesoDatos;
using HD_Dashboard.Modelos.Clientes;

namespace HD_Dashboard.Consultas.Vendedor
{
    public class Dash_Clientes_Facturacion_Detalle
    {
        private string CadenaConexion;
        public Dash_Clientes_Facturacion_Detalle(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDashClientes_Facturacion_Detalle>> Detalle(int idcliente, string linea)
        {
            try
            {
                var parametros = new
                {
                    idcliente,
                    linea
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdlDashClientes_Facturacion_Detalle>("Credito.sp_Obtener_Facturacion_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
