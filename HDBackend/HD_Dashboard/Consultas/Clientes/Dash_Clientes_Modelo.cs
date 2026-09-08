using Dapper;
using HD.AccesoDatos;
using HD_Dashboard.Modelos.Clientes;

namespace HD_Dashboard.Consultas.Clientes
{
    public class Dash_Clientes_Modelo
    {
        private string CadenaConexion;
        public Dash_Clientes_Modelo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlDashClientes_Inventario>> ObtenerModelo(string modelo)
        {
            try
            {
                var parametros = new
                {
                    modelo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdlDashClientes_Inventario>("PixelCode.Inventario.sp_Buscar_Modelo", parametros, commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlDashClientes_Inventario_Detalle>> ObtenerModeloDetalle(string modelo)
        {
            try
            {
                var parametros = new
                {
                    modelo
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdlDashClientes_Inventario_Detalle>("PixelCode.Inventario.sp_Buscar_Modelo_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);

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
