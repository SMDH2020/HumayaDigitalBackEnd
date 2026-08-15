using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_EstimacionVentas
    {
        private string CadenaConexion;
        public AD_EstimacionVentas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_VentasEstimadas_Resultado> ObtenerVentasEstimadas(int anio, int periodo, string sucursal, int usuario)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = anio,
                    periodo = periodo,
                    sucursal = sucursal.ToString(), 
                    usuario = usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                using (var multi = await factory.SQL.QueryMultipleAsync("Ventas.SP_Obtener_Ventas_Estimadas", parametros, commandType: System.Data.CommandType.StoredProcedure))
                {
                    var resultado = new mdl_VentasEstimadas_Resultado
                    {
                        adr = (await multi.ReadAsync<mdl_EstimacionVentas_ADR>()).ToList(),
                        sucursales = (await multi.ReadAsync<mdl_EstimacionVentas_Sucursal>()).ToList(),
                        captura = (await multi.ReadAsync<mdl_EstimacionVentas_Captura>()).FirstOrDefault(),
                        resumen = (await multi.ReadAsync<mdl_EstimacionVentas_Resumen>()).ToList()
                    };
                    factory.SQL.Close();
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_EstimacionVentasDetalle>> ObtenerEstimacionesVentasSucursal(int sucursal, int usuario)
        {
            try
            {
                var parametros = new
                {
                    sucursal = sucursal,
                    usuario = usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_EstimacionVentasDetalle> result = await factory.SQL.QueryAsync<mdl_EstimacionVentasDetalle>("Ventas.SP_Obtener_Estimaciones_Ventas_Sucursal", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> GuardarEstimacionVentas(mdl_GuardarEstimacionVentas mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    json = mdl.detalle
                };
                await factory.SQL.QueryAsync("ventas.SP_GuardarEstimacionVentas", parametros, commandType: System.Data.CommandType.StoredProcedure);
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