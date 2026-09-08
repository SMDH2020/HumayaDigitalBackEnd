using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.ConvenioPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class ADEvidencia_Convenio_Pago_ObtenerDocumento
    {
        private string CadenaConexion;
        public ADEvidencia_Convenio_Pago_ObtenerDocumento(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlConvenio_Pago> Obtener(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                mdlConvenio_Pago result = await factory.SQL.QueryFirstOrDefaultAsync<mdlConvenio_Pago>("Cobranza.sp_Evidencia_Convenio_Pago_ObtenerDocumento", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
