using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisis_Comentarios_Task
    {
        private string CadenaConexion;
        public ADAnalisis_Comentarios_Task(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCAnalisis_Documentacion_View> Guardar(mdlSCAnalisisComentariosTask comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    idproceso = comentario.idproceso,
                    iddocumento= comentario.iddocumento,
                    consecutivo = comentario.consecutivo,
                    comentarios = comentario.comentarios,
                    estatus = comentario.estatus,
                    usuario = comentario.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.SP_Solicitud_Credito_Task_Comentarios_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSCAnalisis_Documentacion_View view = new mdlSCAnalisis_Documentacion_View();
                view.estado = result.Read<mdlSCAnalisis_Pedido_Estado>().FirstOrDefault();
                view.documentacion = result.Read<mdlSCAnalisis_Documentacion>().ToList();

                if (view.estado is null) view.estado = new mdlSCAnalisis_Pedido_Estado();

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
