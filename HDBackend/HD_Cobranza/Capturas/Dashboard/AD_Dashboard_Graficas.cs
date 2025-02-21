using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.Dashboard;
using HD_Cobranza.Modelos.NewFolder;

namespace HD_Cobranza.Capturas.Dashboard
{
    public class AD_Dashboard_Graficas
    {
        private string CadenaConexion;
        public AD_Dashboard_Graficas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Dashboard_Graficas_View> ObtenerGraficas(int ejercicio, int periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                //var parametros = new
                //{
                //    ejercicio,
                //    periodo
                //};
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio11", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo11", periodo, System.Data.DbType.Int16);

                var result = await factory.SQL.QueryMultipleAsync("Cartera_Clientes.Cobranza.Dashboard", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Dashboard_Graficas_View();
                view.total = result.Read<mdl_Dashboard_Total>().ToList();
                view.comportamiento = result.Read<mdl_Dashboard_Comportamiento>().ToList();
                view.comportamiento_responsable = result.Read<mdl_Dashboard_Comportamiento_Responsable>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Dashboard_Graficas_Recuperacion_View> ObtenerGraficasRecuperacion(int ejercicio, int periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                //var parametros = new
                //{
                //    ejercicio,
                //    periodo
                //};
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio11", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo11", periodo, System.Data.DbType.Int16);

                var result = await factory.SQL.QueryMultipleAsync("Cartera_Clientes.Cobranza.Dashboard_Recuperacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Dashboard_Graficas_Recuperacion_View();
                view.objetivo = result.Read<mdl_Dashboard_Objetivo>().ToList();
                view.recuperacion = result.Read<mdl_Dashboard_Recuperacion>().ToList();
                view.recuperacion_responsable = result.Read<mdl_Dashboard_Recuperacion_Responsable>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
