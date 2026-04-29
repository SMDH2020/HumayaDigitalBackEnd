using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos;
using HD_Finanzas.Modelos.RazonesFinancieras;

namespace HD_Finanzas.AccesoDatos
{
    public class AD_InfoModalFactorAbsorcion
    {
        private string CadenaConexion;
        public AD_InfoModalFactorAbsorcion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlFactorAbsorcionModal>> GetFactorAbsorcion()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlFactorAbsorcionModal> modal = await factory.SQL.QueryAsync<mdlFactorAbsorcionModal>("PixelCode.dbo.sp_Dashboard_Finanzas_Inidcador_PostVenta", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return modal;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
