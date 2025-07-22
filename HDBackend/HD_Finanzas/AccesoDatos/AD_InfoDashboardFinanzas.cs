using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos;
using HD_Finanzas.Modelos.Actions;
using HD_Finanzas.Modelos.CostoFinanciamiento;
using HD_Finanzas.Modelos.Estado_Resultados;
using HD_Finanzas.Modelos.NivelInventario;
using HD_Finanzas.Modelos.RazonesFinancieras;
using HD_Finanzas.Modelos.ResultadosSucursal;

namespace HD_Finanzas.AccesoDatos
{
    public class AD_InfoDashboardFinanzas
    {
        private string CadenaConexion;
        public AD_InfoDashboardFinanzas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlInfoDashboardFinanzas> GetDash(int periodoinicio, int periodofin, int ejercicio, string adr, string sucursales, int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodoinicio = periodoinicio,
                    periodofin = periodofin,
                    adr = adr,
                    sucursal = sucursales,
                    usuario = usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("PixelCode.dbo.sp_DashboardFinanzas_HD_Filtrado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
                dashboard.InventarioAntiguedad = result.Read<mdl_Inventario_Antiguedad_Dash>().ToList();
                dashboard.CostoFinanciamiento = result.Read<mdl_Costo_Financiamiento_Dash>().ToList();
                dashboard.ResultadosSucursal = result.Read<mdl_Resultados_Sucursal_Dash>().ToList();
                dashboard.NivelInventario = result.Read<mdl_Nivel_Inventario_Dash>().ToList();
                dashboard.ActualizacionUsuario = result.Read<Fmdl_Actualizacion_Usuario>().ToList();
                dashboard.Permisos = result.Read<mdl_Permisos_Dashboard_Financiero>().ToList();
                dashboard.sucursalesPermiso = result.Read<mdl_Permisos_Dashboard_Sucursales>().ToList();
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
