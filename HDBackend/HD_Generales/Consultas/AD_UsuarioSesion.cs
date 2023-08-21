using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;

namespace HD.Generales.Consultas
{
    public class AD_UsuarioSesion : FactoryConectionBase
    {
        public AD_UsuarioSesion(string _cadenaconexion) : base(_cadenaconexion)
        {

        }
        public async Task<mdlDatosSesion> UsuarioSesion(mdlCodigoSeguridad login)
        {
            try
            {
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
                return new mdlDatosSesion()
                {
                    usuario = usuario,
                    menus = menus,
                    modulos = modulos
                };
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Valido=false;
                return new mdlDatosSesion();
            }
        }
    }
}