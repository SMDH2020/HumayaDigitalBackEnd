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

        public async Task<bool> ModificarFase(string folio, string fase, int usuario)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = folio,
                    fase = fase,
                    usuario = usuario
                };
                await factory.SQL.QueryAsync("Ventas.sp_Editar_Fase_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Cotizaciones>> GetFase(string folio)
        {
            try
            {
                var parametros = new
                {
                    folio = folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Cotizaciones> result = await factory.SQL.QueryAsync<mdl_Listado_Cotizaciones>("Ventas.sp_Obtener_Fases_Info", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
