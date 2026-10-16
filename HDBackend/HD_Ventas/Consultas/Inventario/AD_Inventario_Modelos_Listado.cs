using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.Inventario;

namespace HD_Ventas.Consultas.Inventario
{
    public class AD_Inventario_Modelos_Listado
    {
        private string CadenaConexion;
        public AD_Inventario_Modelos_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Inventario_Modelos_Listado>> Listado()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario_Modelos_Listado> result = await factory.SQL.QueryAsync<mdl_Inventario_Modelos_Listado>("Ventas.sp_Inventario_Modelos_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
