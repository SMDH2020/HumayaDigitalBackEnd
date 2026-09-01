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
    public class ADEvidencia_Convenio_Pago_Guardar
    {
        private string CadenaConexion;
        public ADEvidencia_Convenio_Pago_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlConvenio_Pago mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    folio = mdl.folio,
                    documento = mdl.documento,
                    extension = mdl.extension,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Cobranza.sp_Evidencia_Convenio_Pago_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
