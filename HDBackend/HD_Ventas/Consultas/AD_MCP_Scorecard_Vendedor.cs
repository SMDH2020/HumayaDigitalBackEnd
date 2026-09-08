using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_MCP_Scorecard_Vendedor
    {
        private string CadenaConexion;

        public AD_MCP_Scorecard_Vendedor(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlMCP_Scorecard_Vendedor>> ObtenerScorecard(
            int ejercicio_inicio,
            int periodo_inicio,
            int ejercicio_fin,
            int periodo_fin)
        {
            try
            {
                var parametros = new
                {
                    ejercicio_inicio = ejercicio_inicio,
                    periodo_inicio   = periodo_inicio,
                    ejercicio_fin    = ejercicio_fin,
                    periodo_fin      = periodo_fin
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlMCP_Scorecard_Vendedor> result = await factory.SQL.QueryAsync<mdlMCP_Scorecard_Vendedor>(
                    "Ventas.MCP_Obtener_ScorecardVendedor",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);
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
