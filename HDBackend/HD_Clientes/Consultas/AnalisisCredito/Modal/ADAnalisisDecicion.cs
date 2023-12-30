using Dapper;
using HD.AccesoDatos;
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
    }
}
