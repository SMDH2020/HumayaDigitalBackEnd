using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisis_Solicitud
    {
        private string CadenaConexion;
        public ADAnalisis_Solicitud(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSC_Analisis_SolicitudCredito> BuscarFolio(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                mdlSC_Analisis_SolicitudCredito result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSC_Analisis_SolicitudCredito>("Credito.sp_Obtener_Analisis_Solicitud_Credito", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
