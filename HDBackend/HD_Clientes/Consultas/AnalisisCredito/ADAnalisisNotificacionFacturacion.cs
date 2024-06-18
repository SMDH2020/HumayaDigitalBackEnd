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
        public async Task<mdlAnalisis_Email_Facturacion> GetBody(mdlSCAnalisis_Comentarios comentario)
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
                mdlAnalisis_Email_Facturacion result = await factory.SQL.QueryFirstOrDefaultAsync<mdlAnalisis_Email_Facturacion>("Credito.sp_Analisis_Notificacion_Facturacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result != null)
                    result.comentarios = comentario.comentarios;
                else result = new mdlAnalisis_Email_Facturacion();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
