using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;

namespace HD_Cobranza.Capturas
{
    public class AD_Permiso_Cobranza
    {
        private string CadenaConexion;
        public AD_Permiso_Cobranza(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Permiso_Cobranza> Obtener(int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    usuario = usuario,
                };

                var result = await factory.SQL.QueryAsync<mdl_Permiso_Cobranza>("Cobranza.sp_GetPermiso_MenuCobranza", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result.FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Permisos_Dash_Sucursales>> GetSucursales(int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    usuario = usuario,
                };

                var result = await factory.SQL.QueryAsync<mdl_Permisos_Dash_Sucursales>("Cartera_Clientes.Cobranza.sp_GetPermisosDash_Sucursales", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
