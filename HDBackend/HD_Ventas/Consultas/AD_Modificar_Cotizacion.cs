using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Modificar_Cotizacion
    {
        private string CadenaConexion;
        public AD_Modificar_Cotizacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<bool> ModificarCotizacion(mdl_Modificar_Cotizacion mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    asunto = mdl.asunto,
                    tipo_pago = mdl.tipo_pago,
                    fase = mdl.fase,
                    vigencia = mdl.vigencia,
                    usuario = mdl.usuario,
                    detalle = mdl.detalle
                };
                await factory.SQL.QueryAsync("Ventas.sp_Modificar_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
