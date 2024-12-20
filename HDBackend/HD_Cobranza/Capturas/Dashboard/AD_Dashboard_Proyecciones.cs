using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.Dashboard;
using System.Data;

namespace HD_Cobranza.Capturas.Dashboard
{
    public class AD_Dashboard_Proyecciones
    {
        private string CadenaConexion;
        public AD_Dashboard_Proyecciones(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlProyeccionRecuperarResult> ObtenerProyecciones(int ejercicio, int periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo
                };

                var result = await factory.SQL.QueryMultipleAsync("Cartera_Clientes.Cobranza.sp_Dashboard_Proyecciones", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdlProyeccionRecuperarResult();
                view.listado = result.Read<mdlProyeccionRecuperar>().ToList();
                view.columnas=result.Read<string>().FirstOrDefault();
                view.tipo_cartera=result.Read<string>().FirstOrDefault();
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
