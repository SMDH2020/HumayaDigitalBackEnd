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
        public async Task<mdlSolicitud_Credito_Estado_Resultados> Listado(string folio)
        {
            try
            {
                var parametros = new
                {
                    folio = folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdlSolicitud_Credito_Estado_Resultados result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitud_Credito_Estado_Resultados>("Credito.sp_Solicitud_Credito_Estado_Resultados_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result is null) result = new mdlSolicitud_Credito_Estado_Resultados();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
