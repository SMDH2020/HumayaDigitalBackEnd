using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.ResultadosSucursal;

namespace HD_Finanzas.AccesoDatos
{
    public class FAD_Resultados_Sucursal
    {
        private string CadenaConexion;
        public FAD_Resultados_Sucursal(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Resultado_Sucursal>> Obtener(mdl_Resultados_Sucursal_Filtrado filtro)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    FechaInicio = filtro.fechainicio,
                    fechafin = filtro.fechafin
                    //idusuario= idusuario
                };
                var result = await factory.SQL.QueryAsync<mdl_Resultado_Sucursal>("PixelCode.dbo.sp_ResultadoNegocioporSucursal", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {

                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new
                {
                    errores = ex.Message
                });
            }
        }
    }
}
