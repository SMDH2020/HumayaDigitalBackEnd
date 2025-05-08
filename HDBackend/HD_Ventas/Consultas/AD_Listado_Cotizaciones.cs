using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Listado_Cotizaciones
    {
        private string CadenaConexion;
        public AD_Listado_Cotizaciones(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Listado_Cotizaciones>> Listado(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Cotizaciones> result = await factory.SQL.QueryAsync<mdl_Listado_Cotizaciones>("Ventas.sp_Obtener_Listado_Cotizaciones", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Detalle_Cotizacion>> Detalle(string folio)
        {
            try
            {
                var parametros = new
                {
                    folio = folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Detalle_Cotizacion> result = await factory.SQL.QueryAsync<mdl_Detalle_Cotizacion>("Ventas.sp_Obtener_Detalle_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Obtener_Cotizacion_Editar>> DetalleCotizacion(string folio)
        {
            try
            {
                var parametros = new
                {
                    folio = folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Obtener_Cotizacion_Editar> result = await factory.SQL.QueryAsync<mdl_Obtener_Cotizacion_Editar>("Ventas.sp_Obtener_Cotizacion_Editar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
