using Dapper;
using HD.AccesoDatos;
using Ventas.Modelos;

namespace Ventas.Consultas
{
    public class AD_LineasScorecard_Listado
    {
        private string CadenaConexion;
        public AD_LineasScorecard_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_LineasScorecard>> Listado()
        {
            try
            {
                var parametros = new
                {

                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_LineasScorecard> result = await factory.SQL.QueryAsync<mdl_LineasScorecard>("Ventas.sp_Lineas_Scorecard_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
