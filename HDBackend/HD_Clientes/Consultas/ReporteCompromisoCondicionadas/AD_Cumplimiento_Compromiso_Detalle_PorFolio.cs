using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Reporte_Cumplimiento_Condicionadas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.ReporteCompromisoCondicionadas
{
    public class AD_Cumplimiento_Compromiso_Detalle_PorFolio
    {
        private string CadenaConexion;
        public AD_Cumplimiento_Compromiso_Detalle_PorFolio(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_detalle_folio>> ObtenerdetalleFolio(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                };
                IEnumerable<mdl_detalle_folio> result = await factory.SQL.QueryAsync<mdl_detalle_folio>("Credito.sp_Cumplimiento_Compromiso_Condicionado_Detalle_PorFolio", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
