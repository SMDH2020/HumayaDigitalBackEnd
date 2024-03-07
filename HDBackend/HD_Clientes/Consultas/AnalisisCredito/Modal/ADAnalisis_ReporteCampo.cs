using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito.Modal
{
    public class ADAnalisis_ReporteCampo
    {
        private string CadenaConexion;
        public ADAnalisis_ReporteCampo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCAnalisis_Pedido_Estado> Get(mdlSCAnalisis_Dedidion_View mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    idproceso = mdl.idproceso,
                    usuario = mdl.usuario
                };
                mdlSCAnalisis_Pedido_Estado result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSCAnalisis_Pedido_Estado>("Credito.sp_Analisis_Reporte_Campo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
