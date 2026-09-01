using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class ADConvenio_Pago
    {
        private string CadenaConexion;
        public ADConvenio_Pago(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlFacturasSeleccionadas>> Guardar(mdlConvenio_Pago mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @folio = mdl.folio,
                    @idcliente = mdl.idcliente,
                    @monto = mdl.monto,
                    @tipo_credito= mdl.tipo_credito,
                    @fecha_convenio = mdl.fecha_convenio,
                    @recordatorio = mdl.recordatorio,
                    @fecha_recordatorio = mdl.fecha_recordatorio,
                    @mediocontacto = mdl.mediocontacto,
                    @firma = mdl.firma,
                    @idresponsable=mdl.idresponsable,
                    @descuento = mdl.descuento,
                    @razon_descuento = mdl.razon_descuento,
                    @usuario = mdl.usuario,
                    @detalle = mdl.detalle,
                };

                var result = await factory.SQL.QueryAsync<mdlFacturasSeleccionadas>("Credito.sp_Convenio_pago_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
