using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.CostoFinanciamiento;

namespace HD_Finanzas.AccesoDatos
{
    public class FAD_Costo_Financiamiento
    {
        private string CadenaConexion;
        public FAD_Costo_Financiamiento(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Costo_Financiamiento>> Obtener(mdl_Costo_Financiamiento_Filtrado filtro)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    FechaInicio = filtro.fechainicio,
                    FechaFin = filtro.fechafin
                    //idusuario= idusuario
                };
                var result = await factory.SQL.QueryAsync<mdl_Costo_Financiamiento>("PixelCode.dbo.sp_Obtener_CostoIntegralFinanciamiento", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
