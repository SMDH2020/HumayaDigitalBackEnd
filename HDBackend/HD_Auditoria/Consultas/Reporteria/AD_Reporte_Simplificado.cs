using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Conteo_Piezas;
using HD_Auditoria.Modelos.Reporteria;

namespace HD_Auditoria.Consultas.Reporteria
{
    public class AD_Reporte_Simplificado
    {
        private string CadenaConexion;
        public AD_Reporte_Simplificado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Reporte_Simplificado_View> ReporteSimplificado(string folio)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("folio", folio, System.Data.DbType.String);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Auditoria.sp_GENERA_REPORTE_AUDITORIA_SIMPLIFICADO_P", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Reporte_Simplificado_View mdl = new mdl_Reporte_Simplificado_View();
                mdl.detalle = result.Read<mdl_Reporte_Simplificado_Detalle>().ToList();
                mdl.resumen = result.Read<mdl_Reporte_Simplificado_Resumen>().FirstOrDefault();
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
