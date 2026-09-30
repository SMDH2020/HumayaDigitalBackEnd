using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;

namespace Postventa.Consultas.ReporteMensajeria
{
    public class AD_Filtro_Sucursales_Rol
    {
        private string CadenaConexion;
        public AD_Filtro_Sucursales_Rol(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Filtro_Sucursales_Rol>> ObtenerFiltroSucursal(int usuario)
        {
            try
            {
                //var parametros = new
                //{
                //    ejercicio,
                //    periodo
                //};

                var parametros = new DynamicParameters();
                parametros.Add("usuario", usuario, System.Data.DbType.Int16);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Filtro_Sucursales_Rol> result = await factory.SQL.QueryAsync<mdl_Filtro_Sucursales_Rol>("Postventa.sp_Get_Sucursales_Rol", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
