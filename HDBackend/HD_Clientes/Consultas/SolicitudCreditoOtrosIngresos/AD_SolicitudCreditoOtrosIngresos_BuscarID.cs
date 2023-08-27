using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoOtrosIngresos
{
    public class AD_SolicitudCreditoOtrosIngresos_BuscarID
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoOtrosIngresos_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSolicitud_Credito_Otros_Ingresos> BuscarID(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                mdlSolicitud_Credito_Otros_Ingresos result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitud_Credito_Otros_Ingresos>("Credito.sp_solicitud_credito_otros_ingresos_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
