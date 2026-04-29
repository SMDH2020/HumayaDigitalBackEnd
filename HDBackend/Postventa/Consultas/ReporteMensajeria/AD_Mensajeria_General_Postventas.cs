using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;

namespace Postventa.Consultas.ReporteMensajeria
{
    public class AD_Mensajeria_General_Postventas
    {
        private string CadenaConexion;
        public AD_Mensajeria_General_Postventas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Reporte_Mensajeria_Postventas>> ObtenerReporte(int ejercicio_inicio, int ejercicio_fin, int periodo_inicio, int periodo_fin, string adr, string sucursal, string mostrar, string interes, string motivo, string usuario)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio_inicio", ejercicio_inicio, System.Data.DbType.Int16);
                parametros.Add("ejercicio_fin", ejercicio_fin, System.Data.DbType.Int16);
                parametros.Add("periodo_inicio", periodo_inicio, System.Data.DbType.Int16);
                parametros.Add("periodo_fin", periodo_fin, System.Data.DbType.Int16);
                parametros.Add("adr", adr, System.Data.DbType.String);
                parametros.Add("sucursal", sucursal, System.Data.DbType.String);
                parametros.Add("mostrar", mostrar, System.Data.DbType.String);
                parametros.Add("interes", interes, System.Data.DbType.String);
                parametros.Add("motivo", motivo, System.Data.DbType.String);
                parametros.Add("usuario", usuario, System.Data.DbType.String);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Reporte_Mensajeria_Postventas> result = await factory.SQL.QueryAsync<mdl_Reporte_Mensajeria_Postventas>("Postventa.sp_Obtener_Mensajeria_Postventas", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Accesos_Mensajeria_General>> GetAccesos(int usuario)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("usuario", usuario, System.Data.DbType.Int16);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Accesos_Mensajeria_General> result = await factory.SQL.QueryAsync<mdl_Accesos_Mensajeria_General>("Postventa.sp_get_Accesos_Mensajeria", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
