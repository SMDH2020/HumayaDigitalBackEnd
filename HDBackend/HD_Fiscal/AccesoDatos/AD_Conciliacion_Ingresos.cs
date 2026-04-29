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

        public async Task<mdl_Conciliacion_Ingresos_Analitica_Roles_View> obtenerAnalitica(int ejercicio, int periodo, int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo", periodo, System.Data.DbType.Int16);
                parametros.Add("usuario", usuario, System.Data.DbType.Int16);

                var result = await factory.SQL.QueryMultipleAsync("EQUIP.fiscal.sp_Conciliacion_Ingresos_Analitica_Roles", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Conciliacion_Ingresos_Analitica_Roles_View();
                view.Analitica = result.Read<mdl_Conciliacion_Ingresos_Analitica>().ToList();
                view.Botones = result.Read<mdl_Conciliacion_Ingresos_Analitica_Botones>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> ActualizarConciliacion(mdl_Conciliacion_Actualizar mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = mdl.ejercicio,
                    periodo = mdl.periodo,
                    comentario = mdl.comentario,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("equip.fiscal.sp_Actualiza_Conciliacion_Ingresos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    

        public async Task<bool> AplicarConciliacion(mdl_Conciliacion_Aplicar mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = mdl.ejercicio,
                    periodo = mdl.periodo,
                    detalle = mdl.detalle,
                    comentario = mdl.comentario,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("equip.fiscal.sp_Aplica_Conciliacion_Ingresos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
