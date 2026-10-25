using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.InventarioModelo;

namespace HD_Ventas.Consultas.InventarioModelo
{
    public class AD_InventarioModelo_Listado
    {
        private string CadenaConexion;
        public AD_InventarioModelo_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_InventarioModelo_Listado>> Listado()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_InventarioModelo_Listado> result = await factory.SQL.QueryAsync<mdl_InventarioModelo_Listado>("Ventas.sp_Inventario_Modelos_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
