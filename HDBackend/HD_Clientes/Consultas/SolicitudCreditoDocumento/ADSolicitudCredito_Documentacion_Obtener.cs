using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoDocumento
{
    public class ADSolicitudCredito_Documentacion_Obtener
    {
        private string CadenaConexion;
        public ADSolicitudCredito_Documentacion_Obtener(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlSolicitudCredito_Documentacion>> Obtener(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                IEnumerable<mdlSolicitudCredito_Documentacion> result = await factory.SQL.QueryAsync<mdlSolicitudCredito_Documentacion>("Credito.sp_Solocitud_Credito_Documentacion_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
