using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Listado_Vendedores_Filtro
    {
        private string CadenaConexion;
        public AD_Listado_Vendedores_Filtro(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlListado_Vendedores_Filtro>> ListadoFiltro(int ejercicio, int usuario, int idsucursalfiltro)
        {
            try
            {
                var parametros = new
                {
                    usuario = 8919,
                    idsucursalfiltro = idsucursalfiltro,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlListado_Vendedores_Filtro> result = await factory.SQL.QueryAsync<mdlListado_Vendedores_Filtro>("Ventas.sp_scorecard_Listado_Vendedores_Filtro", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
