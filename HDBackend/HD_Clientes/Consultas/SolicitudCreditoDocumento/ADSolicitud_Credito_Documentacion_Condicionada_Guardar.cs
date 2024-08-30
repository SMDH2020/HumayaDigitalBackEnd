using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;

namespace HD.Clientes.Consultas.SolicitudCreditoDocumento
{
    public class ADSolicitud_Credito_Documentacion_Condicionada_Guardar
    {

        private string CadenaConexion;
        public ADSolicitud_Credito_Documentacion_Condicionada_Guardar(string _cadenaconexion)
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
                IEnumerable<mdlSolicitudCredito_Documentacion> result = await factory.SQL.QueryAsync<mdlSolicitudCredito_Documentacion>("Credito.sp_Soicitud_Credito_Documentacion_Condicionado_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Cargar_Documentacion_Aceptada_Condicionado_View> GuardarDocumentacionAceptada(mdlSolicitudCredito_Documentacion_View view)
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
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Documentacion_Aceptada_Condicionado_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Cargar_Documentacion_Aceptada_Condicionado_View documentosaprobados = new mdl_Cargar_Documentacion_Aceptada_Condicionado_View();
                documentosaprobados.completado = result.Read<mdl_Analisis_Documentacion_Aceptada_Condicionado_Completado>().FirstOrDefault();
                documentosaprobados.mdldatos = result.Read<mdldatos_notificacion>().FirstOrDefault();
                documentosaprobados.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();
                factory.SQL.Close();
                return documentosaprobados;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
