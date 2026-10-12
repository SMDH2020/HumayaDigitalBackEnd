using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.IndicadoresCotizaciones;
using System.Data.SqlClient;

namespace HD.Clientes.Consultas.CRM.IndicadoresCotizaciones
{
    public class AD_IndicadoresCotizaciones_ReporteCotizaciones
    {
        private string CadenaConexion;
        public AD_IndicadoresCotizaciones_ReporteCotizaciones(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Devuelve el objetivo vs las cotizaciones reales por asesor, semana y linea.
        /// El SP regresa el listado ya ordenado por estado, sucursal, vendedor, semana
        /// y orden de linea: no reordenar aqui. El pivoteo lo arma el consumidor.
        /// Los errores definidos por el usuario en SQL (numero mayor o igual a 50000)
        /// son validaciones del SP con mensaje para el usuario final y se devuelven
        /// tal cual como BadRequest.
        /// </summary>
        public async Task<IEnumerable<mdl_IndicadoresCotizaciones_ReporteCotizaciones>> ReporteCotizaciones(int ejercicio, int periodo)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo
                };

                IEnumerable<mdl_IndicadoresCotizaciones_ReporteCotizaciones> result = await factory.SQL.QueryAsync<mdl_IndicadoresCotizaciones_ReporteCotizaciones>("CRM.sp_ObjetivosSemanales_ReporteCotizaciones", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (SqlException ex) when (ex.Number >= 50000)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
