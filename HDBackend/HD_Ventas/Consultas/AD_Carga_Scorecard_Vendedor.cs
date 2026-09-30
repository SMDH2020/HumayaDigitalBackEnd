using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Carga_Scorecard_Vendedor
    {
        private string CadenaConexion;
        public AD_Carga_Scorecard_Vendedor (string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlCarga_Scorecard_Vendedor>> Scorecard(int ejercicio, int usuario, int vendedor)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    usuario = usuario,
                    vendedor = vendedor
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Scorecard_Vendedor> result = await factory.SQL.QueryAsync<mdlCarga_Scorecard_Vendedor>("Ventas.sp_scorecard_Obtener_por_Vendedor", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
