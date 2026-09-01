using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCredito
{
    public class AD_SolicitudCredito_Guardar
    {
        private string CadenaConexion;
        public AD_SolicitudCredito_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlView_Solicitud_Credito> Guardar(mdlSolicitud_Credito mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    idcliente = mdl.idcliente,
                    tipo_solicitud = mdl.tipo_solicitud,
                    importe = mdl.importe,
                    usuario = mdl.usuario,
                    vendedor = mdl.vendedor,
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_solicitud_credito_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSolicitud_Credito_Detalle? detalle = result.Read<mdlSolicitud_Credito_Detalle>().FirstOrDefault();
                mdlSolicitudCredito_Screen? screen = result.Read<mdlSolicitudCredito_Screen>().FirstOrDefault();
                factory.SQL.Close();
                return new mdlView_Solicitud_Credito()
                {
                    solicitud_credito = detalle,
                    config = screen,
                };
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
