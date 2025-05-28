using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;

namespace Postventa.Consultas.Dashboard
{
    public class AD_Obtener_Vencimiento_Garantias
    {
        private string CadenaConexion;
        public AD_Obtener_Vencimiento_Garantias(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Dashboard_Vencimiento_Garantias>> ObtenerVencimientos(int ejercicio, int periodo_inicio, int periodo_fin, string adr, string sucursal)
        {
            try
            {
                //var parametros = new
                //{
                //    ejercicio,
                //    periodo
                //};

                var parametros = new DynamicParameters();
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo_inicio", periodo_inicio, System.Data.DbType.Int16);
                parametros.Add("periodo_fin", periodo_fin, System.Data.DbType.Int16);
                parametros.Add("adr", adr, System.Data.DbType.String);
                parametros.Add("sucursal", sucursal, System.Data.DbType.String);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Dashboard_Vencimiento_Garantias> result = await factory.SQL.QueryAsync<mdl_Dashboard_Vencimiento_Garantias>("PixelCode.Posventa.sp_Obtener_Vencimiento_Garantias", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
