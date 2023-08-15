using Dapper;
using HD_Generales.Autenticate;
using HDSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HD_AccesoDatos.ADAuthenticate
{
    public class ADLogin
    {
        FactoryConnection factory;
        public bool Valido { get; private set; }
        public string Mensaje { get; private set; }
        public ADLogin(string CadenaConexion)
        {
            factory = new FactoryConnection(CadenaConexion);
        }
        public async Task<mdlSesionResult> Autenticacion(mdlALogin login)
        {
            try
            {
                var parametros = new
                {
                    usuario = login.user,
                    password = Encript.HashPassword(login.password)
                };
                var result = await factory.SQL.QueryMultipleAsync("sp_login", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var sesion = new mdlSesionResult();
                sesion.sesion = result.Read<mdlSesion>().FirstOrDefault();
                sesion.autenticacion = result.Read<mdlSesionCodigoAutenticacion>().FirstOrDefault();
                factory.SQL.Close();
                return sesion;
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                return null;
            }
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
                mdlALoginResult usuario = result.Read<mdlALoginResult>().FirstOrDefault();
                IEnumerable<mdlModulos> modulos = result.Read<mdlModulos>().ToList();
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
                return null;
            }
        }
        public async Task<string> ActualizarContraseña(mdlAUpdatePassword login)
        {
            try
            {
                var parametros = new
                {
                    usuario = login.usuario,
                    password = Encript.HashPassword(login.password)
                };
                await factory.SQL.QueryFirstOrDefaultAsync<mdlALoginResult>("sp_ActualizarPassword", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return "Contraseña Actualizada con exito";
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                return null;
            }
        }
    }
}
