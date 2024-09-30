using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF_Condicionado
{
    public class ADSolicitud_Credito_Documentacion_JDF_Guardar
    {
        private string CadenaConexion;
        public ADSolicitud_Credito_Documentacion_JDF_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlSolicitudCredito_Documentacion>> Guardar(mdlSolicitudCredito_Documentacion_View view)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = view.folio,
                    iddocumento = view.iddocumento,
                    documento = view.documento,
                    comentarios = view.comentarios,
                    extension = view.extension,
                    vigencia = view.vigencia,
                    usuario = view.usuario,
                };
                IEnumerable<mdlSolicitudCredito_Documentacion> result = await factory.SQL.QueryAsync<mdlSolicitudCredito_Documentacion>("Credito.sp_Solicitud_Credito_Documentacion_JDF_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
