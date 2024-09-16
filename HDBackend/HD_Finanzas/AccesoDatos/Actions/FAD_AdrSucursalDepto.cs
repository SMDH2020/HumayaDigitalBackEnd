using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.Actions;

namespace HD_Finanzas.AccesoDatos.Actions
{
    public class FAD_AdrSucursalDepto
    {
        private string CadenaConexion;
        public FAD_AdrSucursalDepto(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<FmdlADRScucursalDep>> GetASD(string IdUsuario, string Tipo = "G")
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    IdUsuario = IdUsuario,
                    Tipo = Tipo
                };

                IEnumerable<FmdlADRScucursalDep> asd = await factory.SQL.QueryAsync<FmdlADRScucursalDep>("PixelCode.dbo.SP_Get_ADR_SUCURSAL_DEPARTAMENTO", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return asd;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }
    }
}
