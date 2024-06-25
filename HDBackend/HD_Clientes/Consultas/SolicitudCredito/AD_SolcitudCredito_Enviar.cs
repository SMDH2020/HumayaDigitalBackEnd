using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.SolicitudCredito
{
    public class AD_SolcitudCredito_Enviar
    {
        private string CadenaConexion;
        public AD_SolcitudCredito_Enviar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSolicitudCredito_Enviar_View> Detalle(string folio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Enviar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSolicitudCredito_Enviar_View view = new mdlSolicitudCredito_Enviar_View();
                view.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();
                view.detail = result.Read<mdlSolicitudCredito_Enviar_Details>().FirstOrDefault();
                factory.SQL.Close();
                //if (view.mdlSolicitud == null) view.mdlSolicitud = new mdlSolicitudCredito_Enviar();
                if (view.detail == null) view.detail = new mdlSolicitudCredito_Enviar_Details();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
        public async Task<mdlSolicitudCredito_Enviar_View> Enviar_Condiciones_Operacion(string folio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Pedido_Enviar_Condiciones_Operacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSolicitudCredito_Enviar_View view = new mdlSolicitudCredito_Enviar_View();
                view.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();
                view.detail = result.Read<mdlSolicitudCredito_Enviar_Details>().FirstOrDefault();
                factory.SQL.Close();
                //if (view.mdlSolicitud == null) view.mdlSolicitud = new mdlSolicitudCredito_Enviar();
                if (view.detail == null) view.detail = new mdlSolicitudCredito_Enviar_Details();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
