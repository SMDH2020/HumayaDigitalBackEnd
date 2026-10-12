using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.ReestructurarCredito
{
    public class AD_Reestructurar_Credito_Enviar_Revision
    {
        private string CadenaConexion;
        public AD_Reestructurar_Credito_Enviar_Revision(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdlSolicitudCredito_Enviar_View> Enviar_Solicitud(string folio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Reestructurar_Credito_Enviar_Solicitud_Revision", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
