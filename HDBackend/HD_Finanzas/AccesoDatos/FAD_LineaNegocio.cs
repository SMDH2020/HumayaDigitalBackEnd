using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.Linea_Negocio;

namespace HD_Finanzas.AccesoDatos
{
    public class FAD_LineaNegocio
    {
        private string CadenaConexion;
        public FAD_LineaNegocio(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<Fmdl_Linea_negocio_Esquema_financiero> GetEsquemaByLineadeNegocio(Fmdl_Linea_negocio_filtros vm)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    EjercicioInicio = vm.EjercicioInicio,
                    PeriodoInicio = vm.PeriodoInicio,
                    EjercicioFin = vm.EjercicioFin,
                    PeriodoFin = vm.PeriodoFin,
                    adr = vm.adr,
                    sucursal = vm.sucursales
                };
                var result = await factory.SQL.QueryMultipleAsync("PixelCode.dbo.sp_Esquema_Financiero", parametros, commandType: System.Data.CommandType.StoredProcedure);

                Fmdl_Linea_negocio_Esquema_financiero retorno = new Fmdl_Linea_negocio_Esquema_financiero
                {
                    ventastotales = result.Read<Fmdl_Linea_negocio_ventas_netas_totales>().FirstOrDefault(),
                    margenesbygrupo = result.Read<Fmdl_Linea_negocio_margenes_por_grupo>().FirstOrDefault(),
                    estadoresultados = result.Read<Fmdl_Linea_negocio_estado_resultados>().ToList(),
                    indicadores = result.Read<Fmdl_Linea_negocio_Indicador_margenes>().FirstOrDefault(),
                    margenventasnetas = result.Read<Fmdl_Linea_negocio_ventas_netas_vs_totales>().FirstOrDefault()
                };
                factory.SQL.Close();
                return retorno;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }
    }
}
