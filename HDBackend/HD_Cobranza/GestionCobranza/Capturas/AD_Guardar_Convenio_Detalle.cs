using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Guardar_Convenio_Detalle
    {
        private string CadenaConexion;
        public AD_Guardar_Convenio_Detalle(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Facturas_Seleccionadas>> Guardar(mdl_Convenio_Guardar mdl)
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
                };

                var result = await factory.SQL.QueryAsync<mdl_Facturas_Seleccionadas>("GestionCobranza.sp_Convenio_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
