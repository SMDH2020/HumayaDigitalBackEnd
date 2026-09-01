using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Credito_Condicionado
{
    public class AD_Credito_Condicionado_Notificacion_Correo
    {
        private string CadenaConexion;
        public AD_Credito_Condicionado_Notificacion_Correo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Notificacion_Correo_Solicitud_Condicionada_View> Notificacion( string folio, string usuario, string comentarios, int idproceso)
        {

            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    folio = folio,
                    comentarios = comentarios,
                    usuario = usuario,
                    idproceso = idproceso
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Condicionado_Notificacion_Correo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Notificacion_Correo_Solicitud_Condicionada_View view = new mdl_Notificacion_Correo_Solicitud_Condicionada_View();
                view.mdldatos = result.Read<mdldatos_notificacion>().FirstOrDefault();
                view.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
            finally
            {
                factory.SQL.Close();
            }

        }
    }
}
