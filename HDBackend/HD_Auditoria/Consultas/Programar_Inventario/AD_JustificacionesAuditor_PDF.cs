using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Programar_Inventario
{
    public class AD_JustificacionesAuditor_PDF
    {
        private string CadenaConexion;
        public AD_JustificacionesAuditor_PDF(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_JustificacionesAuditor>> Listado(string folio)
        {
            try
            {
                var parametros = new
                {
                    @folio = folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_JustificacionesAuditor> result = await factory.SQL.QueryAsync<mdl_JustificacionesAuditor>("Auditoria.sp_GENERA_REPORTE_AUDITORIA_JUSTIFICADAS", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
