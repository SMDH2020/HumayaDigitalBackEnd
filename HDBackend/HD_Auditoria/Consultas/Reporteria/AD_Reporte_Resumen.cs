using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Programar_Inventario;
using HD_Auditoria.Modelos.Reporteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Reporteria
{
    public class AD_Reporte_Resumen
    {
        private string CadenaConexion;
        public AD_Reporte_Resumen(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_ReporteResumen_View> Listado(string? folio)
        {
            try
            {
                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@folio", folio, System.Data.DbType.String, System.Data.ParameterDirection.Input, 9);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Auditoria.sp_GENERA_REPORTE_AUDITORIA_RESUMEN", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_ReporteResumen_View listado = new mdl_ReporteResumen_View();
                listado.primer_conteo = result.Read<mdl_Finalizacion_Metricas>().FirstOrDefault();
                listado.segundo_conteo = result.Read<mdl_Finalizacion_Metricas>().FirstOrDefault();
                listado.justificados = result.Read<mdl_Finalizacion_Metricas>().FirstOrDefault();
                //listado.firmas = result.Read<mdl_Firmas_PDF>().FirstOrDefault();
                factory.SQL.Close();
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
