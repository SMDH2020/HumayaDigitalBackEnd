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
                    codigoautenticacion = login.codigoseguridad,
                    oneSignalID = login.oneSignalID
                };
                var result = await factory.SQL.QueryMultipleAsync("sp_Usuario_Sesion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlLoginResult? usuario = result.Read<mdlLoginResult>().FirstOrDefault();
                IEnumerable<mdlModulo> modulos = result.Read<mdlModulo>().ToList();
                IEnumerable<mdlMenu> menus = result.Read<mdlMenu>().ToList();
                IEnumerable<mdlPresas_Niveles> presas = result.Read<mdlPresas_Niveles>().ToList();
                factory.SQL.Close();

                if(usuario == null) { usuario = new mdlLoginResult(); }

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
        public async Task<mdlDatosSesion_Movil> UsuarioSesionMovil(mdlCodigoSeguridad login)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    usuario = login.usuario,
                    codigoautenticacion = login.codigoseguridad,
                    oneSignalID = login.oneSignalID
                };
                var result = await factory.SQL.QueryMultipleAsync("sp_Usuario_Sesion_Movil", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlLoginResult? usuario = result.Read<mdlLoginResult>().FirstOrDefault();
                IEnumerable<mdlPresas_Niveles> presas = result.Read<mdlPresas_Niveles>().ToList();
                IEnumerable <mdl_Rel_Menus_Mobile> menu = result.Read<mdl_Rel_Menus_Mobile>().ToList();
                factory.SQL.Close();

                if (usuario == null) { usuario = new mdlLoginResult(); }

                return new mdlDatosSesion_Movil()
                {
                    usuario = usuario,
                    presas = presas,
                    menu = menu
                };

            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
