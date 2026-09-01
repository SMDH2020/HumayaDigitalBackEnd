using Dapper;
using HD.AccesoDatos;
using HD_Dashboard.Modelos;

namespace HD_Dashboard.Consultas.Vendedor
{
    public class DashScordCardPerfilVendedor
    {
        private string CadenaConexion;
        public DashScordCardPerfilVendedor(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlperfilvendedor>> Listado(string? usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    usuario
                };

                IEnumerable<mdlperfilvendedor> result = await factory.SQL.QueryAsync<mdlperfilvendedor>("sp_Vendedores_ScordCard", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
