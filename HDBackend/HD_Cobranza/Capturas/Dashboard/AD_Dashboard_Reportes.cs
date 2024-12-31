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

        public async Task<IEnumerable<mdl_Dashboard_Reporte_Grafica_Total>> ReporteGraficaRecuperacion(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza)
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
                IEnumerable<mdl_Dashboard_Reporte_Grafica_Total> result = await factory.SQL.QueryAsync<mdl_Dashboard_Reporte_Grafica_Total>("Cartera_Clientes.Cobranza.Dashboard_Recuperacion_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Dashboard_Reporte_Grafica_Total>> ReporteGraficaObjetivos(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string categoria)
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
                    responsable_cartera = responsable_cobranza,
                    categoria = categoria
                };
                IEnumerable<mdl_Dashboard_Reporte_Grafica_Total> result = await factory.SQL.QueryAsync<mdl_Dashboard_Reporte_Grafica_Total>("Cartera_Clientes.Cobranza.Dashboard_Total_Clientes_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 60);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Dashboard_Recuperacion_Mensual_Detalle>> ReporteGraficaProyeccionMensual(int ejercicio, int periodo, string mes, string sucursales, string adr, string tipo_cartera)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    mes = mes,
                    sucursales = sucursales,
                    adr = adr, 
                    tipo_cartera = tipo_cartera

                };
                IEnumerable<mdl_Dashboard_Recuperacion_Mensual_Detalle> result = await factory.SQL.QueryAsync<mdl_Dashboard_Recuperacion_Mensual_Detalle>("Cartera_Clientes.Cobranza.sp_Proyeccion_Recuperacion_Mensual_tipo_cartera", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Dashboard_Recuperacion_Mensual_Detalle>> ReporteGraficaProyeccionTotal(int ejercicio, int periodo, string mes, string sucursales, string adr)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    mes = mes,
                    sucursales = sucursales,
                    adr = adr
                };
                IEnumerable<mdl_Dashboard_Recuperacion_Mensual_Detalle> result = await factory.SQL.QueryAsync<mdl_Dashboard_Recuperacion_Mensual_Detalle>("Cartera_Clientes.Cobranza.sp_Proyeccion_Recuperacion_Mensual", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
