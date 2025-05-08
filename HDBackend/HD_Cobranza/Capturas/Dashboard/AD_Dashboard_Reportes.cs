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
        public async Task<IEnumerable<mdl_Dashboard_Reporte_Grafica_Total>> ReporteGraficaTotal(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cartera, string adr, string sucursales)
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
                    responsable_cartera = responsable_cartera,
                    adr = adr,
                    sucursales = sucursales
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

        public async Task<IEnumerable<mdl_Dashboard_Reporte_Grafica_Total>> ReporteGraficaRecuperacion(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cartera, string adr, string sucursales)
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
                    responsable_cartera = responsable_cartera,
                    adr = adr,
                    sucursales = sucursales
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

        public async Task<IEnumerable<mdl_Dashboard_Reporte_Grafica_Total>> ReporteGraficaObjetivos(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cartera, string categoria, string adr, string sucursales)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                //var parametros = new
                //{
                //    ejercicio11 = ejercicio,
                //    periodo11 = periodo,
                //    tipo_grafica = tipo_grafica,
                //    tipo_cartera = tipo_cartera,
                //    estado_cartera = estado_cartera,
                //    responsable_cartera = responsable_cartera,
                //    categoria = categoria,
                //    adr = adr,
                //    sucursales = sucursales
                //};
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio11", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo11", periodo, System.Data.DbType.Int16);
                parametros.Add("tipo_grafica", tipo_grafica, System.Data.DbType.String);
                parametros.Add("tipo_cartera", tipo_cartera, System.Data.DbType.String);
                parametros.Add("estado_cartera", estado_cartera, System.Data.DbType.String);
                parametros.Add("responsable_cartera", responsable_cartera, System.Data.DbType.String);
                parametros.Add("categoria", categoria, System.Data.DbType.String);
                parametros.Add("adr", adr, System.Data.DbType.String);
                parametros.Add("sucursales", sucursales, System.Data.DbType.String);
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

        public async Task<IEnumerable<mdl_Reporte_Proyeccion_Recuperacion_Mensual_tipo_cartera>> ProyeccionMensualPorTipoCartera(string mes, string sucursales, string adr, string tipo_cartera)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    mes = mes,
                    sucursales = sucursales,
                    adr = adr,
                    tipo_cartera = tipo_cartera

                };
                IEnumerable<mdl_Reporte_Proyeccion_Recuperacion_Mensual_tipo_cartera> result = await factory.SQL.QueryAsync<mdl_Reporte_Proyeccion_Recuperacion_Mensual_tipo_cartera>("Cartera_Clientes.Cobranza.sp_Reporte_Proyeccion_Recuperacion_Mensual_tipo_cartera", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Reporte_Proyeccion_Recuperacion_Mensual_tipo_cartera>> ProyeccionMensualTotal(string mes, string sucursales, string adr)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    mes = mes,
                    sucursales = sucursales,
                    adr = adr

                };
                IEnumerable<mdl_Reporte_Proyeccion_Recuperacion_Mensual_tipo_cartera> result = await factory.SQL.QueryAsync<mdl_Reporte_Proyeccion_Recuperacion_Mensual_tipo_cartera>("Cartera_Clientes.Cobranza.sp_Reporte_Proyeccion_Recuperacion_Mensual_Total", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
