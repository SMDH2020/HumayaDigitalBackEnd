using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCredito
{
    public class AD_SolicitudCredito_Detalle
    {
        private string CadenaConexion;
        public AD_SolicitudCredito_Detalle(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlView_Solicitud_Credito> Detalle(string folio,string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Obtener_SolicitudCredito_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSolicitud_Credito_Detalle? detalle = result.Read<mdlSolicitud_Credito_Detalle>().FirstOrDefault();
                mdlSolicitudCredito_Screen? screen = result.Read<mdlSolicitudCredito_Screen>().FirstOrDefault();
                factory.SQL.Close();
                return new mdlView_Solicitud_Credito()
                {
                    solicitud_credito = detalle,
                    config = screen,
                };
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
