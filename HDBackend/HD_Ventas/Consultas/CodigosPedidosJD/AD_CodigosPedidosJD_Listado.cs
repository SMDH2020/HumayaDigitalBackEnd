using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.CodigosPedidosJD;

namespace HD_Ventas.Consultas.CodigosPedidosJD
{
    public class AD_CodigosPedidosJD_Listado
    {
        private string CadenaConexion;
        public AD_CodigosPedidosJD_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_CodigosPedidosJD_Listado>> Listado()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_CodigosPedidosJD_Listado> result = await factory.SQL.QueryAsync<mdl_CodigosPedidosJD_Listado>("Ventas.sp_Codigos_PedidosJD_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
