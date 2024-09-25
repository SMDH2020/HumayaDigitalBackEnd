using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;

namespace HD.Clientes.Consultas.Credito_Condicionado
{
    public class AD_Credito_Condicionado_Solicitud_Guardar
    {
        private string CadenaConexion;
        public AD_Credito_Condicionado_Solicitud_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdlSC_Credito_Condicionado> Guardar(mdl_fecha_compromiso_documentos_detalle mdl,string usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion,true);
            try
            {

                foreach(mdl_fecha_compromiso_documentos detalle in mdl.detalle)
                {
                    if(detalle.enviar_revision==true && detalle.tiene_documentacion==false
                        && detalle.fecha_compromiso < DateTime.Now)
                    {
                        throw new Exception($"La fecha para entrega del documento {detalle.documento} no puede ser menor a la fecha actual");
                    }
                }

                string folio = null;
                AD_Credito_Condicionado_Fecha_Comprimiso_Documentacion_Vendedor_Guardar datos = new AD_Credito_Condicionado_Fecha_Comprimiso_Documentacion_Vendedor_Guardar(CadenaConexion);
                foreach (mdl_fecha_compromiso_documentos detalle in mdl.detalle)
                {
                    detalle.usuario = usuario;
                    if (detalle.enviar_revision == false)
                    {
                        detalle.fecha_compromiso = new DateTime(1900, 1, 1);
                        detalle.comentarios = "No se entregara documento";
                    }
                    else if (detalle.tiene_documentacion )
                    {
                        detalle.fecha_compromiso = DateTime.Now.Date;
                        detalle.comentarios = "Documento Entregado";
                        
                    }
                    else
                    {
                        detalle.comentarios = "Fecha de entrega del documento: " + detalle.fecha_compromiso.ToString("dd-MM-yyyy");
                    }
                    await datos.Guardar(detalle);

                    // Extrae el folio del primer elemento
                    if (folio == null)
                    {
                        folio = detalle.folio;
                    }
                }
                datos.Commit();
                AD_Credito_Condicionado_Fecha_Compromiso_Guardar compromiso = new AD_Credito_Condicionado_Fecha_Compromiso_Guardar(CadenaConexion);
                var result = await compromiso.Guardar(folio, usuario,mdl.comentarios);

                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
