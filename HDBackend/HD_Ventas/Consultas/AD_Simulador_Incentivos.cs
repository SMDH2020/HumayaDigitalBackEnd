using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Simulador_Incentivos
    {
        private string CadenaConexion;
        public AD_Simulador_Incentivos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlSimulador_Roles_Usuario>> Obtener_Roles_Usuario(int usuario)
        {
            try
            {
                var parametros = new
                {
                    idusuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlSimulador_Roles_Usuario> result = await factory.SQL.QueryAsync<mdlSimulador_Roles_Usuario>("sp_obtener_roles_simulador", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
