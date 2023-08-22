using Dapper;
using HD.AccesoDatos;
using HD.Generales.Autenticate;
using HD.Security;

namespace HD.Generales.Consultas
{
    public class AD_UpdatePassword 
    {
        private string CadenaConexion;
        public AD_UpdatePassword(string _cadenaconexion) 
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<string> ActualizarContraseña(mdlUpdatePassword login)
        {
            try
            {
                FactoryConection factory = new(CadenaConexion);

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
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
