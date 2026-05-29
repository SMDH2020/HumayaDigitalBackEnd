using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Programar_Inventario;
using HD_Auditoria.Modelos.Reporteria;

namespace HD_Auditoria.Consultas.Reporteria
{
    public class AD_Reporte_Primer_Conteo
    {
        private string CadenaConexion;
        public AD_Reporte_Primer_Conteo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_ReporteSimplificado_View> Listado(string? folio)
        {
            try
            {
                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@folio", folio, System.Data.DbType.String, System.Data.ParameterDirection.Input, 9);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Auditoria.sp_GENERA_REPORTE_AUDITORIA_PRIMER_CONTEO", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_ReporteSimplificado_View listado = new mdl_ReporteSimplificado_View();
                listado.diferencias = result.Read<mdl_Finalizacion_Diferencias>().ToList();
                listado.info = result.Read<mdl_Finalizacion_Metricas>().FirstOrDefault();
                listado.firmas = result.Read<mdl_Firmas_PDF>().FirstOrDefault();
                factory.SQL.Close();
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdl_Reporte_Primer_Conteo_View> ReportePrimerConteo(string folio)

        {

            try
            {

                var parametros = new DynamicParameters();

                parametros.Add("folio", folio, System.Data.DbType.String);

                FactoryConection factory = new FactoryConection(CadenaConexion);

                var result = await factory.SQL.QueryMultipleAsync("Auditoria.sp_GENERA_REPORTE_AUDITORIA_PRIMER_CONTEO", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdl_Reporte_Primer_Conteo_View mdl = new mdl_Reporte_Primer_Conteo_View();

                mdl.detalle = result.Read<mdl_Reporte_Primer_Conteo_Detalle>().ToList();

                mdl.resumen = result.Read<mdl_Reporte_Primer_Conteo_Resumen>().FirstOrDefault();

                factory.SQL.Close();

                return mdl;

            }

            catch (System.Exception ex)

            {

                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });

            }

        }
    }
}
