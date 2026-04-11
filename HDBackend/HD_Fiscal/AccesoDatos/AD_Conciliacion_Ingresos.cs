using Dapper;
using HD.AccesoDatos;
using HD.Fiscal.Modelos;


namespace HD.Fiscal.AccesoDatos
{
    public class AD_Conciliacion_Ingresos
    {
        private string CadenaConexion;
        public AD_Conciliacion_Ingresos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Conciliacion_Ingresos_Invoice>> obtenerInvoice(int ejercicio, int periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo", periodo, System.Data.DbType.Int16);
                IEnumerable<mdl_Conciliacion_Ingresos_Invoice> result = await factory.SQL.QueryAsync<mdl_Conciliacion_Ingresos_Invoice>("EQUIP.fiscal.sp_Conciliacion_Ingresos_Invoice", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Conciliacion_Ingresos_Analitica>> obtenerAnalitica(int ejercicio, int periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo", periodo, System.Data.DbType.Int16);
                IEnumerable<mdl_Conciliacion_Ingresos_Analitica> result = await factory.SQL.QueryAsync<mdl_Conciliacion_Ingresos_Analitica>("EQUIP.fiscal.sp_Conciliacion_Ingresos_Analitica", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
