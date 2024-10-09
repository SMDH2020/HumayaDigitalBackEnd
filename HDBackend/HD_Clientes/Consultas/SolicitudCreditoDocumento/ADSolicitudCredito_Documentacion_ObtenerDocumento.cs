using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCreditoDocumento
{
    public class ADSolicitudCredito_Documentacion_ObtenerDocumento
    {
        private string CadenaConexion;
        public ADSolicitudCredito_Documentacion_ObtenerDocumento(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSolicitudCredito_Documentacion_View> Obtener(string folio,int iddocumento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    iddocumento
                };
                mdlSolicitudCredito_Documentacion_View result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitudCredito_Documentacion_View>("Credito.sp_Solicitud_Credito_Documentacion_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlSolicitudCredito_Documentacion_View> ObtenerResultadoOperacion(string folio, int iddocumento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    iddocumento
                };
                mdlSolicitudCredito_Documentacion_View result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitudCredito_Documentacion_View>("Credito.sp_Solicitud_Credito_Documentacion_Obtener_Resultado_Operacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlSolicitudCredito_Documentacion_View> ObtenerFactura(string folio,int registro, int iddocumento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    registro,
                    iddocumento
                };
                mdlSolicitudCredito_Documentacion_View result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitudCredito_Documentacion_View>("Credito.sp_Obtener_Documento_FacturaNota", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<RPT_Result> ObtenerPEdido(string folio, int iddocumento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    iddocumento
                };
                RPT_Result result = await factory.SQL.QueryFirstOrDefaultAsync<RPT_Result>("Credito.sp_Solicitud_Credito_Documentacion_Pedido_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<RPT_Result> ObtenerPagare(string folio, int iddocumento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    iddocumento
                };
                RPT_Result result = await factory.SQL.QueryFirstOrDefaultAsync<RPT_Result>("Credito.sp_Solicitud_Credito_Documentacion_Pagare_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
