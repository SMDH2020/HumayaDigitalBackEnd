using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisiCreditoMhusa
    {
        private string CadenaConexion;
        public ADAnalisiCreditoMhusa(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCAnalisis_Pedido_Estado> ValidarCondicionesOperacion(string folio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                mdlSCAnalisis_Pedido_Estado result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSCAnalisis_Pedido_Estado>("Credito.sp_Analisis_Validar_Condiciones_Operacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlAnalisis_Mhusa> GuardarValidacion(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    comentarios = comentario.comentarios,
                    estatus = comentario.estatus,
                    idproceso = comentario.idproceso,
                    usuario = comentario.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.SP_Validar_Condiciones_Operacion_Comentarios_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlAnalisis_Mhusa mhusa = new mdlAnalisis_Mhusa();
                mhusa.mdldatos = result.Read<mdldatos_notificacion>().FirstOrDefault();
                mhusa.estado = result.Read<mdlSCAnalisis_Pedido_Estado>().FirstOrDefault();
                //mhusa.documentacion = result.Read<mdlSCAnalisis_Documentacion>().ToList();
                mhusa.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();
                factory.SQL.Close();
                return mhusa;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlAnalisis_Mhusa> Guardar(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    iddocumento = comentario.iddocumento,
                    comentarios = comentario.comentarios,
                    estatus = comentario.estatus,
                    usuario = comentario.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.SP_Solicitud_Credito_Task_Comentarios_Mhusa_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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

        public async Task<mdl_Analisis_Documentacion_Aceptada_Condicionado_View> GuardarAnalisisDocumentosAceptadosCondicionados(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    iddocumento = comentario.iddocumento,
                    comentarios = comentario.comentarios,
                    estatus = comentario.estatus,
                    usuario = comentario.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.SP_Solicitud_Credito_Task_Analizar_Documentacion_Condicionado_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Analisis_Documentacion_Aceptada_Condicionado_View documentosaprobados = new mdl_Analisis_Documentacion_Aceptada_Condicionado_View();
                documentosaprobados.completado = result.Read<mdl_Analisis_Documentacion_Aceptada_Condicionado_Completado>().FirstOrDefault();
                documentosaprobados.documentos = result.Read<mdl_Analisis_100_detalle>().ToList();
                documentosaprobados.mdldatos = result.Read<mdldatos_notificacion>().FirstOrDefault();
                documentosaprobados.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();
                factory.SQL.Close();
                return documentosaprobados;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdlAnalisis_Mhusa> GuardarComentarioCondicionado(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    iddocumento = comentario.iddocumento,
                    comentarios = comentario.comentarios,
                    estatus = comentario.estatus,
                    usuario = comentario.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.SP_Solicitud_Credito_Condicionado_Task_Comentarios_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
        public async Task<mdlAnalisis_Mhusa> GuardarOtorgamiento(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    iddocumento = comentario.iddocumento,
                    comentarios = comentario.comentarios,
                    estatus = comentario.estatus,
                    usuario = comentario.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.SP_Solicitud_Credito_Task_Comentarios_Otorgamiento_Mhusa_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
        public async Task<mdl_Analisis_100_view> GuardarOtorgamientoComentariosCondicionado(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    iddocumento = comentario.iddocumento,
                    comentarios = comentario.comentarios,
                    estatus = comentario.estatus,
                    usuario = comentario.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.SP_Solicitud_Credito_Task_Comentarios_Condicionado_Finanzas", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Analisis_100_view view = new mdl_Analisis_100_view();
                view.encabezado = result.Read<mdl_Analisis_100_encabezado>().FirstOrDefault();
                view.detalle = result.Read<mdl_Analisis_100_detalle>().ToList();

                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlAnalisis_Mhusa> GuardarComentariosAnalisis(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    comentarios = comentario.comentarios,
                    estatus = comentario.estatus,
                    usuario = comentario.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.SP_Solicitud_Credito_Task_Comentarios_Mhusa_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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

        public async Task<mdlSCAnalisis_Documentacion_View> GuardarComentarioAnalisis(string folio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Analisis_Documentacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
