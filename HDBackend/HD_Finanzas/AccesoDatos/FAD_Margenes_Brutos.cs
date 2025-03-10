using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.Margenes;

namespace HD_Finanzas.AccesoDatos
{
    public class FAD_Margenes_Brutos
    {
        private string CadenaConexion;
        public FAD_Margenes_Brutos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Margenes_Brutos>> GetMargenesBrutos(int ejercicio, string periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo
                };
                IEnumerable<mdl_Margenes_Brutos> gastosvs = await factory.SQL.QueryAsync<mdl_Margenes_Brutos>("PixelCode.dbo.sp_Get_MArgenes_Semaforo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return gastosvs;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
