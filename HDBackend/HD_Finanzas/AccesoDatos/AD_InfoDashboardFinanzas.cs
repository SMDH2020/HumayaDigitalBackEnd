using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos;
using HD_Finanzas.Modelos.RazonesFinancieras;

namespace HD_Finanzas.AccesoDatos
{
    public class AD_InfoDashboardFinanzas
    {
        private string CadenaConexion;
        public AD_InfoDashboardFinanzas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlInfoDashboardFinanzas> GetDash()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                };
                var result = await factory.SQL.QueryMultipleAsync("PixelCode.dbo.sp_DashboardFinanzas_HD", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlInfoDashboardFinanzas dashboard = new mdlInfoDashboardFinanzas();
                dashboard.estadoresultadoreal = result.Read<mdlEstadoResultadoReal>().FirstOrDefault();
                dashboard.estadoresultadoproyectado = result.Read<mdlEstadoResultadoProyectado>().FirstOrDefault();
                dashboard.ventasnetas = result.Read<mdlVentasNetas>().ToList();
                dashboard.ventasnetasAnterior = result.Read<mdlVentasNetasAnterior>().ToList();
                dashboard.ventasnetasproyectada = result.Read<mdlVentasNetasProyectadas>().ToList();
                dashboard.gastos = result.Read<mdlGastos>().ToList();
                dashboard.gastosanterior = result.Read<mdlGastosAnterior>().ToList();
                dashboard.gastosproyectados = result.Read<mdlGastosProyectados>().ToList();
                dashboard.balancegeneral = result.Read<mdlBalanceGeneral>().ToList();
                dashboard.razonesfinancieras = result.Read<mdlRazonesFinancieras>().ToList();
                factory.SQL.Close();
                return dashboard;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
