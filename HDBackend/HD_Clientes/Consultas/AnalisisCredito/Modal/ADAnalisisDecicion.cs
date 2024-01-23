using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito.Modal
{
    public class ADAnalisisDecicion
    {
        private string CadenaConexion;
        public ADAnalisisDecicion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCAnalisis_Decicion> Get(mdlSCAnalisis_Dedidion_View mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio=mdl.folio,
                    idproceso=mdl.idproceso,
                    usuario=mdl.usuario,
                    responsable = mdl.responsable
                };
                mdlSCAnalisis_Decicion result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSCAnalisis_Decicion>("Credito.sp_Analisis_Decicion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlJDFAnalisis_Decicion_un_documento> GetUndocumento(mdlJDFAnalisis_Un_Documento_Decicion_View mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    idproceso = mdl.idproceso,
                    iddocumento=mdl.iddocumento,
                    usuario = mdl.usuario
                };
                mdlJDFAnalisis_Decicion_un_documento result = await factory.SQL.QueryFirstOrDefaultAsync<mdlJDFAnalisis_Decicion_un_documento>("Credito.sp_Analisis_JDF_un_Documento", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
