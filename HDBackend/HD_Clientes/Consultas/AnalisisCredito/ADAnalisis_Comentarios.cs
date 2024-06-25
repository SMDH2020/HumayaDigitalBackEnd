using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisis_Comentarios
    {
        private string CadenaConexion;
        public ADAnalisis_Comentarios(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlAnalisis_Mhusa> Guardar(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    idproceso=comentario.idproceso,
                    consecutivo=comentario.consecutivo,
                    comentarios=comentario.comentarios,
                    estatus=comentario.estatus,
                    usuario = comentario.usuario
                };
                var result=await factory.SQL.QueryMultipleAsync("Credito.SP_Solicitud_Credito_Comentarios_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlAnalisis_Mhusa mhusa = new mdlAnalisis_Mhusa();
                mhusa.mdldatos = result.Read<mdldatos_notificacion>().FirstOrDefault();
                mhusa.estado = result.Read<mdlSCAnalisis_Pedido_Estado>().FirstOrDefault();
                mhusa.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();
                factory.SQL.Close();
                return mhusa;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlAnalisis_Mhusa> GuardarOtorgamiento(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    comentarios = comentario.comentarios,
                    usuario = comentario.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.SP_Solicitud_Credito_Comentarios_Otorgamiento_Mhusa_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlAnalisis_Mhusa mhusa = new mdlAnalisis_Mhusa();
                mhusa.mdldatos = result.Read<mdldatos_notificacion>().FirstOrDefault();
                mhusa.estado = result.Read<mdlSCAnalisis_Pedido_Estado>().FirstOrDefault();
                mhusa.documentacion = result.Read<mdlSCAnalisis_Documentacion>().ToList();
                mhusa.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();
                factory.SQL.Close();
                return mhusa;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
