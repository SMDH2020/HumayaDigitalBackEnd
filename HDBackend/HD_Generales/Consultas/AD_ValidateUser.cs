using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;

namespace HD.Generales.Consultas
{
    public class AD_ValidateUser
    {
        private string CadenaConexion;
        public AD_ValidateUser(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlDatosSesion> UsuarioSesion(string? idusuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    usuario=idusuario
                };
                var result = await factory.SQL.QueryMultipleAsync("sp_Usuario_Sesion_Validar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlLoginResult? usuario = result.Read<mdlLoginResult>().FirstOrDefault();
                IEnumerable<mdlModulo> modulos = result.Read<mdlModulo>().ToList();
                IEnumerable<mdlMenu> menus = result.Read<mdlMenu>().ToList();
                IEnumerable<mdlPresas_Niveles> presas = result.Read<mdlPresas_Niveles>().ToList();
                factory.SQL.Close();

                if (usuario == null) { usuario = new mdlLoginResult(); }

                return new mdlDatosSesion()
                {
                    usuario = usuario,
                    menus = menus,
                    modulos = modulos,
                    presas = presas
                };

            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
