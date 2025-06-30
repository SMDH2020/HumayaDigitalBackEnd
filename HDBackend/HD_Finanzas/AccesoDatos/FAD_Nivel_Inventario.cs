using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.NivelInventario;
namespace HD_Finanzas.AccesoDatos
{
    public class FAD_Nivel_Inventario
    {
        private string CadenaConexion;
        public FAD_Nivel_Inventario(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Nivel_Inventario>> Obtener(mdl_Nivel_Inventario_Filtrado filtro)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    Ejercicio = filtro.Ejercicio,
                    Periodo = filtro.Periodo,
                    //ADR = filtro.adr,
                    Sucursales = filtro.sucursal,
                    departamentos = filtro.departamentos,
                    usuario = filtro.usuario
                };
                var result = await factory.SQL.QueryAsync<mdl_Nivel_Inventario>("PixelCode.dbo.sp_Obtener_Inventario_HD", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
