using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoEstadoResultados
{
    public class AD_SolicitudCreditoEstadoResultados_Listado
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoEstadoResultados_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlSolicitud_Credito_Estado_Resultados>> Listado(string folio)
        {
            try
            {
                var parametros = new
                {
                    folio = folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlSolicitud_Credito_Estado_Resultados> result = await factory.SQL.QueryAsync<mdlSolicitud_Credito_Estado_Resultados>("Credito.sp_solicitud_credito_estado_resultados_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
