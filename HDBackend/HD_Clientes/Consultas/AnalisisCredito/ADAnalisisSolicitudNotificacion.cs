using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.SC_Analisis.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisisSolicitudNotificacion
    {
        private string CadenaConexion;
        public ADAnalisisSolicitudNotificacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSC_Analisis_Credito> GetBody(mdlSCAnalisisComentariosTask comentario)
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
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Analisis_Documentacion_Notificacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSC_Analisis_Credito view = new mdlSC_Analisis_Credito();
                view.email = result.Read<mdlAnalisis_Email>().FirstOrDefault();
                view.documentacion = result.Read<mdlSCAnalisis_Documentacion>().ToList();

                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
