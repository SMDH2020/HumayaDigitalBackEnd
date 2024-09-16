using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisisNotificacionFacturacion
    {
        private string CadenaConexion;
        public ADAnalisisNotificacionFacturacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlAnalisis_Email_View> GetBody(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    idproceso = comentario.idproceso,
                    estatus = comentario.estatus,
                };
                var view = await factory.SQL.QueryMultipleAsync("Credito.sp_Analisis_Notificacion_Facturacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlAnalisis_Email_View result = new mdlAnalisis_Email_View();
                result.notificacion = view.Read<mdlCorreo_Notificacion>().ToList();
                result.detalle = view.Read<mdlAnalisis_Email>().FirstOrDefault();
                factory.SQL.Close();
                if (result.detalle == null) result.detalle = new mdlAnalisis_Email();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
