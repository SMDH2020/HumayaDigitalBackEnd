using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoSiniestros
{
    public class AD_SolicitudCreditoSiniestros_BuscarID
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoSiniestros_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSolicitud_Credito_Siniestros> BuscarID(short registro)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    registro
                };
                mdlSolicitud_Credito_Siniestros result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitud_Credito_Siniestros>("Credito.sp_solicitud_credito_siniestros_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
