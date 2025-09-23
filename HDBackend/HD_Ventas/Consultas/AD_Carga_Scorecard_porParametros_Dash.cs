using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;


namespace HD_Ventas.Consultas
{
    public class AD_Carga_Scorecard_porParametros_Dash
    {
        private string CadenaConexion;
        public AD_Carga_Scorecard_porParametros_Dash(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlCarga_Scorecard_porVendedor_Dash>> Scorecard(int region, string sucursal, string usuario, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual, int sesion)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario,
                    region = region,
                    sucursal = sucursal,
                    ejercicioinicio = ejercicioinicio,
                    periodoinicio = periodoinicio,
                    ejercicio = ejercicio,
                    mes_actual = mes_actual,
                    sesion = sesion
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Scorecard_porVendedor_Dash> result = await factory.SQL.QueryAsync<mdlCarga_Scorecard_porVendedor_Dash>("Ventas.Obtener_Scorecard_porParametro_Nuevo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }


        public async Task<IEnumerable<mdl_Carga_Scorecard_Vendedor_Detalle>> VendedorDetalle(int region, string sucursal, string usuario, int ejercicio, int sesion)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario,
                    region = region,
                    sucursal = sucursal,
                    ejercicio = ejercicio,
                    sesion = sesion
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Carga_Scorecard_Vendedor_Detalle> result = await factory.SQL.QueryAsync<mdl_Carga_Scorecard_Vendedor_Detalle>("Ventas.Obtener_Scorecard_Detalle_Vendedor_Periodos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlCarga_Scorecard_porVendedor_Dash_TablaAsesor>> Scorecard_TablaAsesor(int region, string? sucursal, string? usuario, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual, int sesion)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario,
                    region = region,
                    sucursal = sucursal,
                    ejercicioinicio = ejercicioinicio,
                    periodoinicio = periodoinicio,
                    ejercicio = ejercicio,
                    mes_actual = mes_actual,
                    sesion = sesion
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Scorecard_porVendedor_Dash_TablaAsesor> result = await factory.SQL.QueryAsync<mdlCarga_Scorecard_porVendedor_Dash_TablaAsesor>("Ventas.Obtener_Scorecard_porParametro_Tabla_Asesor_Nuevo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdlCarga_Scorecard_porVendedor_Dash_TablaAsesor_Importes>> Scorecard_TablaAsesor_importes(int region, string sucursal, string usuario, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual, int sesion)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario,
                    region = region,
                    sucursal = sucursal,
                    ejercicioinicio = ejercicioinicio,
                    periodoinicio = periodoinicio,
                    ejercicio = ejercicio,
                    mes_actual = mes_actual,
                    sesion = sesion
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Scorecard_porVendedor_Dash_TablaAsesor_Importes> result = await factory.SQL.QueryAsync<mdlCarga_Scorecard_porVendedor_Dash_TablaAsesor_Importes>("Ventas.Obtener_Scorecard_porParametro_Tabla_Asesor_Importes", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
