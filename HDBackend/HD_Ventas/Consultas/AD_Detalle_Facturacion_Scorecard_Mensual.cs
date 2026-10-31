using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Detalle_Facturacion_Scorecard_Mensual
    {
        private string CadenaConexion;
        public AD_Detalle_Facturacion_Scorecard_Mensual(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDetalle_Facturacion_Scorecard>> Get(string linea, int ejercicio, int periodo, string adr, string sucursal, string vendedor)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    linea = linea,
                    ejercicio = ejercicio,
                    periodo = periodo,
                    adr = adr,
                    sucursal = sucursal,
                    vendedor = vendedor
                };
                IEnumerable<mdlDetalle_Facturacion_Scorecard> result = await factory.SQL.QueryAsync<mdlDetalle_Facturacion_Scorecard>("Ventas.Obtener_Detalle_Facturacion_Scorecard_Mensual", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
