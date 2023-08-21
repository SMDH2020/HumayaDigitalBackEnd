using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;
using HD.Security;

namespace HD.Generales.Consultas
{
    public class AD_UpdatePassword : FactoryConectionBase
    {
        public AD_UpdatePassword(string _cadenaconexion) : base(_cadenaconexion)
        {

        }
        public async Task<string> ActualizarContraseña(mdlUpdatePAssword login)
        {
            try
            {
                var parametros = new
                {
                    usuario = login.usuario,
                    password = Encript.HashPassword(login.password)
                };
                await factory.SQL.QueryFirstOrDefaultAsync<mdlLoginResult>("sp_ActualizarPassword", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return "Contraseña Actualizada con exito";
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                Valido = false;
                return "";
            }
        }
    }
}