using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Programar_Inventario;
using HD_Auditoria.Modelos.Reporteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Programar_Inventario
{
    public class AD_ReporteSegundoconteo_PDF
    {
        private string CadenaConexion;
        public AD_ReporteSegundoconteo_PDF(string _cadenaconexion)
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

                var result = await factory.SQL.QueryMultipleAsync("Auditoria.sp_GENERA_REPORTE_AUDITORIA_SEGUNDO_CONTEO", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
    }
}
