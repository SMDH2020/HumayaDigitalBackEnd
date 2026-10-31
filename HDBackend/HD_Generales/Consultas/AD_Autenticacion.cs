using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;
using HD.Security;
using System.Data;

namespace HD.Generales.Consultas
{
    public class AD_Autenticacion 
    {
        private string CadenaConexion;
        public AD_Autenticacion(string _cadenaconexion) 
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSesionResult> Autenticar(mdlLogin _login)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                
                var parametros = new
                {
                    usuario = _login.user,
                    password = Encript.HashPassword(_login.password)
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
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlSesionResult> AutenticarMovil(mdlLogin_Movil _login)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);

                var parametros = new
                {
                    usuario = _login.user,
                    password = Encript.HashPassword(_login.password),
                    IMEI = _login.IMEI
                };

                var result = await factory.SQL.QueryMultipleAsync("sp_login_Movil", parametros, commandType: System.Data.CommandType.StoredProcedure);

                var sesion = new mdlSesionResult();
                sesion.sesion = result.Read<mdlSesion>().FirstOrDefault();
                sesion.autenticacion = result.Read<mdlSesionCodigoAutenticacion>().FirstOrDefault();

                factory.SQL.Close();

                return sesion;

            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
