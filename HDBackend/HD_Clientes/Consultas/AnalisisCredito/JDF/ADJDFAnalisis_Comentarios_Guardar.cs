using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF
{
    public class ADJDFAnalisis_Comentarios_Guardar
    {
        private string CadenaConexion;
        public ADJDFAnalisis_Comentarios_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Analisis_Un_Documento_View> Guardar(mdlJDFAnalisiComentarios_Guardar_View comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    idproceso = comentario.idproceso,
                    iddocumento = comentario.iddocumento,
                    documento = comentario.documento,
                    extension = comentario.extension,
                    comentarios = comentario.comentarios,
                    estatus = comentario.estatus,
                    usuario = comentario.usuario
                };
                var retorno = await factory.SQL.QueryMultipleAsync("Credito.SP_Solicitud_Credito_JDF_Comentarios_Guardar_Evento", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Analisis_Un_Documento_View result = new mdl_Analisis_Un_Documento_View();
                result.documento = retorno.Read<mdlJDFAnalisis_Decicion_un_documento>().FirstOrDefault();
                result.mdlSolicitud = retorno.Read<mdlSolicitudCredito_Enviar>().ToList();
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
