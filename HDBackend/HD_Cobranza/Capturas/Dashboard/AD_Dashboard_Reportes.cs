using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Cobranza.Modelos.Dashboard;

namespace HD_Cobranza.Capturas.Dashboard
{
    public class AD_Dashboard_Reportes
    {
        private string CadenaConexion;
        public AD_Dashboard_Reportes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Dashboard_Reporte_Grafica_Total>> ReporteGraficaTotal(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    tipo_grafica = tipo_grafica,
                    tipo_cartera = tipo_cartera,
                    estado_cartera = estado_cartera,
                    responsable_cartera = responsable_cobranza
                };
                IEnumerable<mdl_Dashboard_Reporte_Grafica_Total> result = await factory.SQL.QueryAsync<mdl_Dashboard_Reporte_Grafica_Total>("Cartera_Clientes.Cobranza.Dashboard_Total_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
