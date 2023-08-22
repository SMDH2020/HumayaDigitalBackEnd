using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;

namespace HD.Generales.Consultas
{
    public class AD_UsuarioSesion
    {
        private string CadenaConexion;
        public AD_UsuarioSesion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlDatosSesion> UsuarioSesion(mdlCodigoSeguridad login)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    usuario = login.usuario,
                    codigoautenticacion = login.codigoseguridad
                };
                var result = await factory.SQL.QueryMultipleAsync("sp_Usuario_Sesion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlLoginResult? usuario = result.Read<mdlLoginResult>().FirstOrDefault();
                IEnumerable<mdlModulo> modulos = result.Read<mdlModulo>().ToList();
                IEnumerable<mdlMenu> menus = result.Read<mdlMenu>().ToList();
                factory.SQL.Close();

                if(usuario == null) { usuario = new mdlLoginResult(); }

                return new mdlDatosSesion()
                {
                    usuario = usuario,
                    menus = menus,
                    modulos = modulos
                };

            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
