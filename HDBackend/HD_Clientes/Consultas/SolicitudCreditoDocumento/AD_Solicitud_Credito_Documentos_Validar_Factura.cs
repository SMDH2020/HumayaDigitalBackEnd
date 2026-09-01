using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.SolicitudCreditoDocumento
{
    public class AD_Solicitud_Credito_Documentos_Validar_Factura
    {

            private string CadenaConexion;
            public AD_Solicitud_Credito_Documentos_Validar_Factura(string _cadenaconexion)
            {
                CadenaConexion = _cadenaconexion;
            }
            public async Task<IEnumerable<mdlSolicitudCredito_Documentacion_View>> Obtener(string folio, int iddocumento)
            {
                try
                {
                    FactoryConection factory = new FactoryConection(CadenaConexion);
                    var parametros = new
                    {
                        folio,
                        iddocumento
                    };
                    IEnumerable<mdlSolicitudCredito_Documentacion_View> result = await factory.SQL.QueryAsync<mdlSolicitudCredito_Documentacion_View>("Credito.sp_Solicitud_Credito_Documentos_Validar_Factura", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
