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
        public async Task<IEnumerable<mdl_AnalisisInventarioModelos_Reporte>> Reporte(int ejercicio_inicio, int periodo_inicio, int ejercicio_fin, int periodo_fin, int comparativo, int usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    ejercicio_inicio = ejercicio_inicio,
                    periodo_inicio = periodo_inicio,
                    ejercicio_fin = ejercicio_fin,
                    periodo_fin = periodo_fin,
                    comparativo = comparativo,
                    usuario = usuario
                };
                IEnumerable<mdl_AnalisisInventarioModelos_Reporte> result = await factory.SQL.QueryAsync<mdl_AnalisisInventarioModelos_Reporte>("Ventas.sp_Analisis_Inventario_Reporte", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
