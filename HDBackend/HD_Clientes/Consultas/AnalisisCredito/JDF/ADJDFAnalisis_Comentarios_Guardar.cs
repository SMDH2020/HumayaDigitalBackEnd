using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.JDF;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF
{
    public class ADJDFAnalisis_Comentarios_Guardar
    {
        private string CadenaConexion;
        public ADJDFAnalisis_Comentarios_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlJDFAnalisis_Decicion_un_documento> Guardar(mdlJDFAnalisiComentarios_Guardar_View comentario)
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
                mdlJDFAnalisis_Decicion_un_documento result = await factory.SQL.QueryFirstOrDefaultAsync<mdlJDFAnalisis_Decicion_un_documento>("Credito.SP_Solicitud_Credito_JDF_Comentarios_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
