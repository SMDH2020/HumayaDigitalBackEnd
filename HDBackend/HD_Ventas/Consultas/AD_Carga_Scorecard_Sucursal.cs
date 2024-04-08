using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Carga_Scorecard_Sucursal
    {
        private string CadenaConexion;
        public AD_Carga_Scorecard_Sucursal(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlCarga_Scorecard_Sucursal>> Scorecard(int ejercicio, int sucursal)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    idsucursal = sucursal
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Scorecard_Sucursal> result = await factory.SQL.QueryAsync<mdlCarga_Scorecard_Sucursal>("Ventas.sp_scorecard_Obtener_por_Sucursal", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
