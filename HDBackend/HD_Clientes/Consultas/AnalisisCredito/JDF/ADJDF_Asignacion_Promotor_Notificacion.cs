//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HD.Clientes.Consultas.AnalisisCredito.JDF
//{
//    public class ADJDF_Asignacion_Promotor_Notificacion
//    {

//        private string CadenaConexion;
//        public ADJDF_Asignacion_Promotor_Notificacion(string _cadenaconexion)
//        {
//            CadenaConexion = _cadenaconexion;
//        }
//        public async Task<mdlAnalisis_Email> GetBody(mdlSCAnalisis_Comentarios comentario)
//        {
//            try
//            {
//                FactoryConection factory = new FactoryConection(CadenaConexion);
//                var parametros = new
//                {
//                    folio = comentario.folio,
//                    idproceso = comentario.idproceso,
//                    estatus = comentario.estatus,
//                };
//                mdlAnalisis_Email result = await factory.SQL.QueryFirstOrDefaultAsync<mdlAnalisis_Email>("Credito.sp_Analisis_Notificacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
//                factory.SQL.Close();
//                if (result != null)
//                    result.comentarios = comentario.comentarios;
//                else result = new mdlAnalisis_Email();
//                return result;
//            }
//            catch (System.Exception ex)
//            {
//                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
//            }
//        }
//    }
//}
