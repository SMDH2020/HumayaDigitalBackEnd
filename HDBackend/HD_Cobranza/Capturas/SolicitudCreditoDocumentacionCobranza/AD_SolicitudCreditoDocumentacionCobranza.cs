using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.SolicitudCreditoDocumentacionCobranza;
using System.Data.SqlClient;

namespace HD_Cobranza.Capturas.SolicitudCreditoDocumentacionCobranza
{
    public class AD_SolicitudCreditoDocumentacionCobranza
    {
        private string CadenaConexion;
        public AD_SolicitudCreditoDocumentacionCobranza(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlSolicitudCreditoDocumentacionCobranza>> Listado(DateTime fecha_inicio, DateTime fecha_fin, string sucursal, string adr, string tipo_solicitud)
        {
            try
            {
                var parametros = new
                {
                    @Fecha_Inicio = fecha_inicio,
                    @Fecha_Fin = fecha_fin,
                    @Sucursal = sucursal,
                    @ADR = adr,
                    @Tipo_Solicitud = tipo_solicitud
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlSolicitudCreditoDocumentacionCobranza> result = await factory.SQL.QueryAsync<mdlSolicitudCreditoDocumentacionCobranza>("Credito.sp_Solicitud_Credito_Documentacion_Cobranza", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (SqlException ex) when (ex.Number >= 50000)
            {
                // Validaciones que el SP lanza con THROW (50001 fechas nulas,
                // 50002 rango invertido). El mensaje es para el usuario.
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlSolicitudCreditoDocumentacionCobranza_Detalle>> Detalle(string folio)
        {
            try
            {
                var parametros = new
                {
                    @Folio = folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlSolicitudCreditoDocumentacionCobranza_Detalle> result = await factory.SQL.QueryAsync<mdlSolicitudCreditoDocumentacionCobranza_Detalle>("Credito.sp_Solicitud_Credito_Documentacion_Cobranza_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                // Lista vacia = el folio no existe. No es error.
                return result;
            }
            catch (SqlException ex) when (ex.Number >= 50000)
            {
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
