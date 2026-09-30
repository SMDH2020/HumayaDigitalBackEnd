using Dapper;
using HD.AccesoDatos;
using HD.Fiscal.Modelos;

namespace HD.Fiscal.AccesoDatos
{
    public class AD_Listado_Incidencias_Anticipos
    {
        private string CadenaConexion;
        public AD_Listado_Incidencias_Anticipos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_rel_anticipos>> obtenerAnticipos (string v_ref, string serie_fiscal, string importe)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("v_ref", v_ref, System.Data.DbType.String);
                parametros.Add("serie_fiscal", serie_fiscal, System.Data.DbType.String);
                parametros.Add("importe", importe, System.Data.DbType.String);
                IEnumerable<mdl_Listado_rel_anticipos> result = await factory.SQL.QueryAsync<mdl_Listado_rel_anticipos>("EQUIP.fiscal.sp_Obtener_Listado_Posibles_Anticipos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_rel_cancelaciones>> obtenerCancelaciones(string v_ref, string serie_fiscal, string importe)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("v_ref", v_ref, System.Data.DbType.String);
                parametros.Add("serie_fiscal", serie_fiscal, System.Data.DbType.String);
                parametros.Add("importe", importe, System.Data.DbType.String);
                IEnumerable<mdl_Listado_rel_cancelaciones> result = await factory.SQL.QueryAsync<mdl_Listado_rel_cancelaciones>("EQUIP.fiscal.sp_Obtener_Listado_Posibles_Cancelaciones", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
