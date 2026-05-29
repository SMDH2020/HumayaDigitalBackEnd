using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Reporteria;

namespace HD_Auditoria.Consultas.Reporteria
{
    public class AD_Reporte_Segundo_Conteo
    {
        private string CadenaConexion;
        public AD_Reporte_Segundo_Conteo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Reporte_Primer_Conteo_View> ReporteSegundoConteo(string folio)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("folio", folio, System.Data.DbType.String);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Auditoria.sp_GENERA_REPORTE_AUDITORIA_SEGUNDO_CONTEO_P", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
