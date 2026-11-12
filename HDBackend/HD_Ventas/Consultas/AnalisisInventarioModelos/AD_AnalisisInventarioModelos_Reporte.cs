using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.AnalisisInventarioModelos;
using System.Data.SqlClient;

namespace HD_Ventas.Consultas.AnalisisInventarioModelos
{
    public class AD_AnalisisInventarioModelos_Reporte
    {
        private string CadenaConexion;
        public AD_AnalisisInventarioModelos_Reporte(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Los errores definidos por el usuario en SQL (numero mayor o igual a 50000) son validaciones
        /// del SP con mensaje para el usuario final y se devuelven tal cual como BadRequest.
        /// </summary>
        public async Task<mdl_AnalisisInventarioModelos_Reporte_View> Reporte(int inicio_tendencia, int fin_tendencia, int inicio_resto, int fin_resto, int inicio_proyeccion, int fin_proyeccion, int comparativo, string sucursal, string adr, int usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    inicio_tendencia = inicio_tendencia,
                    fin_tendencia = fin_tendencia,
                    inicio_resto = inicio_resto,
                    fin_resto = fin_resto,
                    inicio_proyeccion = inicio_proyeccion,
                    fin_proyeccion = fin_proyeccion,
                    comparativo = comparativo,
                    sucursal = sucursal,
                    adr = adr,
                    usuario = usuario
                };
                var resultSets = await factory.SQL.QueryMultipleAsync("Ventas.sp_Analisis_Inventario_Reporte", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_AnalisisInventarioModelos_Reporte_View result = new mdl_AnalisisInventarioModelos_Reporte_View();
                result.modelos = resultSets.Read<mdl_AnalisisInventarioModelos_Reporte>().ToList();
                result.detalle = resultSets.Read<mdl_AnalisisInventarioModelos_Reporte_Detalle>().ToList();
                factory.SQL.Close();
                return result;
            }
            catch (SqlException ex) when (ex.Number >= 50000)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
