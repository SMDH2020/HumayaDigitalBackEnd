using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Modal;
using HD.Clientes.Modelos.SC_Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisisNotificacion
    {
        private string CadenaConexion;
        public ADAnalisisNotificacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlAnalisis_Email> GetBody(mdlSCAnalisis_Comentarios comentario)
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
                mdlAnalisis_Email result = await factory.SQL.QueryFirstOrDefaultAsync<mdlAnalisis_Email>("Credito.sp_Analisis_Notificacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result != null)
                    result.comentarios = comentario.comentarios;
                else result = new mdlAnalisis_Email();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlAnalisisAutorizacionFacturacion_Email> GetBodyAutorizacionFacturacion(mdlSCAnalisis_Comentarios comentario)
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
                mdlAnalisisAutorizacionFacturacion_Email result = await factory.SQL.QueryFirstOrDefaultAsync<mdlAnalisisAutorizacionFacturacion_Email>("Credito.sp_Analisis_Notificacion_Autorizacion_Facturacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result != null)
                    result.comentarios = comentario.comentarios;
                else result = new mdlAnalisisAutorizacionFacturacion_Email();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
