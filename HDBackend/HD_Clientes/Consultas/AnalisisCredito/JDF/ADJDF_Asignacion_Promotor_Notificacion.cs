using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF
{
    public class ADJDF_Asignacion_Promotor_Notificacion
    {

        private string CadenaConexion;
        public ADJDF_Asignacion_Promotor_Notificacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlAnalisis_Email_View> GetBody(mdlJDFAnalisis_Asignar_Promotor_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Asignacion_promotor_Notificacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlAnalisis_Email_View view = new mdlAnalisis_Email_View();
                view.notificacion = result.Read<mdlCorreo_Notificacion>().ToList();
                view.detalle = result.Read<mdlAnalisis_Email>().FirstOrDefault();
                factory.SQL.Close();
                if (view.detalle == null) view.detalle = new mdlAnalisis_Email();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
