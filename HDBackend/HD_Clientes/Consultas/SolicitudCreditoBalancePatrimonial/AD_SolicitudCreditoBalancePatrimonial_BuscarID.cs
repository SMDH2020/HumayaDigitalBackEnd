using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoBalancePatrimonial
{
    public class AD_SolicitudCreditoBalancePatrimonial_BuscarID
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoBalancePatrimonial_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSolicitud_Credito_Balance_Patrimonial> BuscarID(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                mdlSolicitud_Credito_Balance_Patrimonial result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitud_Credito_Balance_Patrimonial>("Credito.sp_solicitud_credito_balance_patrimonial_obtenerporID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
