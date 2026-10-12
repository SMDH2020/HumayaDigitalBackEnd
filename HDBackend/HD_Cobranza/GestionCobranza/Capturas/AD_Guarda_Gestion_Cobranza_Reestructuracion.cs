using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Guarda_Gestion_Cobranza_Reestructuracion
    {
        private string CadenaConexion;
        public AD_Guarda_Gestion_Cobranza_Reestructuracion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Gestion_Cobranza_Reestructuracion>> Guardar(mdl_Gestion_Cobranza_Reestructuracion mdl, string email, string password)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @folio = mdl.folio,
                    @idcliente = mdl.idcliente,
                    @monto = mdl.monto,
                    @tipo_credito = mdl.tipo_credito,
                    @fecha_convenio = mdl.fecha_convenio,
                    @recordatorio = mdl.recordatorio,
                    @fecha_recordatorio = mdl.fecha_recordatorio,
                    @mediocontacto = mdl.mediocontacto,
                    @firma = mdl.firma,
                    @idresponsable = mdl.idresponsable,
                    @descuento = mdl.descuento,
                    @razon_descuento = mdl.razon_descuento,
                    @usuario = mdl.usuario,
                    @detalle = mdl.detalle,
                    @gestion = mdl.gestion,
                    @objecion = mdl.objecion,
                    @comentarios = mdl.comentarios,
                    @volvercontactar = mdl.volvercontactar,
                    @fechavolveracontactar = mdl.fechavolveracontactar,
                    @saldo = mdl.saldo,
                    @moratorios = mdl.moratorios,
                    @interespactado = mdl.interespactado,
                    @total = mdl.total,
                    @comentario_reestructuracion = mdl.comentario_reestructuracion,
                    @documento_reestructuracion = mdl.documento_reestructuracion,
                    @extension = mdl.extension
                };

                var result = await factory.SQL.QueryAsync<mdl_Gestion_Cobranza_Reestructuracion>("GestionCobranza.sp_Guardar_Gestion_Cobranza", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                //NotificacionReestructuracionGestion.Enviar(config, CadenaConexion, mdl.documento_reestructuracion, mdl.idcliente, mdl.comentario_reestructuracion);
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        // El envio del correo vive ahora en HD.Notifications.Cobranza.NotificacionReestructuracionGestion.Enviar(...)
        // (HD_Cobranza no puede referenciar a HD_Notifications).
    }
}
