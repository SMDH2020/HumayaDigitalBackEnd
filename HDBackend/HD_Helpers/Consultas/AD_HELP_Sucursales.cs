using Dapper;
using HD.AccesoDatos;
using HD_Helpers.Modelos;

namespace HD_Helpers.Consultas
{
    public class AD_HELP_Sucursales
    {
        private string CadenaConexion;
        public AD_HELP_Sucursales(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_HELP_Sucursales>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var result = await factory.SQL.QueryAsync<mdl_HELP_Sucursales>("sp_Obtener_Sucursales_Activas", commandType: System.Data.CommandType.StoredProcedure);
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
