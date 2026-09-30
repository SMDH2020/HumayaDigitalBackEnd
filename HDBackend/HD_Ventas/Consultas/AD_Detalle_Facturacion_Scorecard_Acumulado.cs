using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Detalle_Facturacion_Scorecard_Acumulado
    {
        private string CadenaConexion;
        public AD_Detalle_Facturacion_Scorecard_Acumulado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDetalle_Facturacion_Scorecard>> Get(string linea, int ejercicioinicio, int periodoinicio, int ejerciciofin, int periodofin, string adr, string sucursal, string vendedor)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    linea,
                    ejercicioinicio,
                    periodoinicio,
                    ejerciciofin,
                    periodofin,
                    adr,
                    sucursal,
                    vendedor
                };
                IEnumerable<mdlDetalle_Facturacion_Scorecard> result = await factory.SQL.QueryAsync<mdlDetalle_Facturacion_Scorecard>("Ventas.Obtener_Detalle_Facturacion_Scorecard_Acumulado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
