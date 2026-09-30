using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoBalancePatrimonial
{
    public class AD_SolicitudCreditoBalancePatrimonial_Listado
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoBalancePatrimonial_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSolicitud_Credito_Balance_Patrimonial> Listado(string folio)
        {
            try
            {
                var parametros = new
                {
                    folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdlSolicitud_Credito_Balance_Patrimonial result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitud_Credito_Balance_Patrimonial>("Credito.sp_Solicitud_Credito_Balance_Patrimonial_obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result is null) result = new mdlSolicitud_Credito_Balance_Patrimonial();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
