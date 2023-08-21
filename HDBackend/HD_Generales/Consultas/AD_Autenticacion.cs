using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;
using HD.Security;

namespace HD.Generales.Consultas
{
    public class AD_Autenticacion : FactoryConectionBase
    {
        public AD_Autenticacion(string _cadenaconexion) : base(_cadenaconexion)
        {

        }
        public async Task<mdlSesionresult> Autenticar(mdlLogin _login)
        {
            try
            {
                var parametros = new
                {
                    usuario = _login.user,
                    password = Encript.HashPassword(_login.password)
                };
                var result = await factory.SQL.QueryMultipleAsync("sp_login", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var sesion = new mdlSesionresult();
                sesion.sesion = result.Read<mdlSesion>().FirstOrDefault();
                sesion.autenticacion = result.Read<mdlSesionCodigoAutenticacion>().FirstOrDefault();
                factory.SQL.Close();
                return sesion;
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Valido=false;
                return new mdlSesionresult();
            }
        }
    }
}