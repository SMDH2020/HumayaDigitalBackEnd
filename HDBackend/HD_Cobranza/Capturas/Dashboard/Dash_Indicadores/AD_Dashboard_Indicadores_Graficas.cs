using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.Dashboard;
using HD_Cobranza.Modelos.Dashboard.Dash_Indicadores;

namespace HD_Cobranza.Capturas.Dashboard.Dash_Indicadores
{
    public class AD_Dashboard_Indicadores_Graficas
    {
        private string CadenaConexion;
        public AD_Dashboard_Indicadores_Graficas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Dashboard_View> ObtenerGraficas(int ejercicio, int periodo, string adr, string sucursales)
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
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo", periodo, System.Data.DbType.Int16);
                parametros.Add("adr", adr, System.Data.DbType.String);
                parametros.Add("sucursal", sucursales, System.Data.DbType.String);

                var result = await factory.SQL.QueryMultipleAsync("Cartera_Clientes.Cobranza.sp_Dashboard_Indicadores", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Dashboard_View();
                view.header = result.Read<mdl_Dashboard_Header>().ToList();
                view.total = result.Read<mdl_Dashboard_TotalCartera>().ToList();
                view.total_estados = result.Read<mdl_Dashboard_TotalCartera_Estados>().ToList();
                view.recuperacion = result.Read<mdl_Dashboard_RecuperacionCartera>().ToList();
                view.gestion = result.Read<mdl_Dashboard_GestionCobranza>().ToList();
                view.pedidos = result.Read<mdl_Dashboard_PedidosFacturados>().ToList();
                view.juridico = result.Read<mdl_Dashboard_ClientesJuridico>().ToList();
                view.mensajes = result.Read<mdl_Dashboard_MensajesAutomaticos>().ToList();
                view.listado = result.Read<mdl_Dashboard_ProyeccionesRecuperar>().ToList();
                view.columnas = result.Read<string>().FirstOrDefault();
                view.tipo_cartera = result.Read<string>().FirstOrDefault();
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
