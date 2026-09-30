using Dapper;
using HD.AccesoDatos;
using HD_Dashboard.Modelos;

namespace HD_Dashboard.Consultas
{
    public class AD_Seleccionar_Scorecard
    {
        private string CadenaConexion;
        public AD_Seleccionar_Scorecard(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Seleccionar_Scorecard>> usuario(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Seleccionar_Scorecard> result = await factory.SQL.QueryAsync<mdl_Seleccionar_Scorecard>("dashboard.sp_SeleccionarScorecard", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
