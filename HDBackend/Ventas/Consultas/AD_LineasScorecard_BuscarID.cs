using Dapper;
using HD.AccesoDatos;
using Ventas.Modelos;

namespace Ventas.Consultas
{
    public class AD_LineasScorecard_BuscarID
    {
        private string CadenaConexion;
        public AD_LineasScorecard_BuscarID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_LineasScorecard> BuscarID(int idlinea)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idlinea
                };
                mdl_LineasScorecard result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_LineasScorecard>("Ventas.sp_Lineas_Scorecard_BuscarID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
