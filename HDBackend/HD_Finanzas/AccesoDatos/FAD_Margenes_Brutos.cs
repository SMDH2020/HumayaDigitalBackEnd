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
        public async Task<mdl_Margenes_Brutos_View> GetMargenesBrutos(int ejercicio, string periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo
                };
                var result = await factory.SQL.QueryMultipleAsync("PixelCode.dbo.sp_Get_MArgenes_Semaforo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Margenes_Brutos_View();
                view.margenes = result.Read<mdl_Margenes_Brutos>().ToList();
                view.guias = result.Read<mdl_Margenes_Brutos_Guias>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
