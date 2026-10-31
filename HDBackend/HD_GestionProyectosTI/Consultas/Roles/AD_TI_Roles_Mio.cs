using Dapper;
using HD.AccesoDatos;
using HD_GestionProyectosTI.Modelos;

namespace HD_GestionProyectosTI.Consultas.Roles
{
    public class AD_TI_Roles_Mio
    {
        private string CadenaConexion;
        public AD_TI_Roles_Mio(string _cadenaconexion) { CadenaConexion = _cadenaconexion; }

        public async Task<IEnumerable<mdl_TI_Rol>> Mio(int usuarioId)
        {
            try
            {
                var parametros = new { usuarioId };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_TI_Rol> result = await factory.SQL.QueryAsync<mdl_TI_Rol>(
                    "dbo.sp_TI_Roles_Mio", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
