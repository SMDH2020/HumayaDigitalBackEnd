using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoOtrosIngresos
{
    public class AD_SolicitudCreditoOtrosIngresos_Listado
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoOtrosIngresos_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlSolicitud_Credito_Otros_Ingresos>> Listado(string folio)
        {
            try
            {
                var parametros = new
                {
                    folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlSolicitud_Credito_Otros_Ingresos> result = await factory.SQL.QueryAsync<mdlSolicitud_Credito_Otros_Ingresos>("Credito.sp_solicitud_credito_otros_ingresos_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
