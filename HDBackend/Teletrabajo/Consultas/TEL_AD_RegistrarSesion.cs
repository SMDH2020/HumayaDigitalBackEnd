using Dapper;
using HD.AccesoDatos;
using Teletrabajo.Modelos;

namespace Teletrabajo.Consultas
{
    public class TEL_AD_RegistrarSesion
    {
        private string CadenaConexion;
        public TEL_AD_RegistrarSesion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<TEL_mdl_Lecturas>> PrimerRegistro(TEL_mdl_InfoSesion mdl)
        {
            try
            {
                var parametros = new
                {
                    idusuario = mdl.usuario,
                    serie = mdl.serie,
                    modelo = mdl.modelo,
                    marca = mdl.marca,
                    mac = mdl.mac
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<TEL_mdl_Lecturas> result = await factory.SQL.QueryAsync<TEL_mdl_Lecturas>("HumayaDigital_Usuarios.dbo.sp_Usuarios_Teletrabajo_Login", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<TEL_mdl_Lecturas>> Acceso(TEL_mdl_InfoSesion mdl)
        {
            try
            {
                var parametros = new
                {
                    idusuario = mdl.usuario,
                    serie = mdl.serie,
                    modelo = mdl.modelo,
                    marca = mdl.marca,
                    mac = mdl.mac
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<TEL_mdl_Lecturas> result = await factory.SQL.QueryAsync<TEL_mdl_Lecturas>("HumayaDigital_Usuarios.dbo.sp_Usuarios_Teletrabajo_Lecturas", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
