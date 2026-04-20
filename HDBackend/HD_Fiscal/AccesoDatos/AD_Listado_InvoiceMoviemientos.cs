using Dapper;
using HD.AccesoDatos;
using HD.Fiscal.Modelos;


namespace HD.Fiscal.AccesoDatos
{
    public class AD_Listado_InvoiceMoviemientos
    {
        private string CadenaConexion;
        public AD_Listado_InvoiceMoviemientos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Listados_InvoiceMovimientos_View> ObtenerListados(int ejercicio, int periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo", periodo, System.Data.DbType.Int16);

                var result = await factory.SQL.QueryMultipleAsync("EQUIP.fiscal.sp_Obtener_Listado_Invoice_Movimientos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Listados_InvoiceMovimientos_View();
                view.Invoice = result.Read<mdl_Listado_Invoice>().ToList();
                view.Movimientos = result.Read<mdl_Listado_MovimientoContable>().ToList();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Correccion_Incidencias_View> ObtenerCorreccionIncidencias(int ejercicio, int periodo, int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo", periodo, System.Data.DbType.Int16);
                parametros.Add("usuario", usuario, System.Data.DbType.Int16);

                var result = await factory.SQL.QueryMultipleAsync("EQUIP.fiscal.sp_Obtener_Listados_Incidencias", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Correccion_Incidencias_View();
                view.Invoice = result.Read<mdl_Listado_Invoice>().ToList();
                view.Ventas_Internas = result.Read<mdl_Listado_Incidencia_VentasInternas>().ToList();
                view.Descuentos_Notimbrados = result.Read<mdl_Listado_Incidencia_DescuentosNoTimbrados>().ToList();
                view.Facturacion_NoRegistrada = result.Read<mdl_Listado_Incidencia_Facturacion_NoRegistrada_EnVentas>().ToList();
                view.Facturación_SinUuid = result.Read<mdl_Listado_Incidencia_Facturacion_SinUuid>().ToList();
                view.Reversas = result.Read<mdl_Reversas_UUID_Vigente>().ToList();
                view.botones = result.Read<mdl_Conciliacion_Ingresos_Analitica_Botones>().FirstOrDefault();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Invoice>> obtenerInvoice(int ejercicio, int periodo)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("ejercicio", ejercicio, System.Data.DbType.Int16);
                parametros.Add("periodo", periodo, System.Data.DbType.Int16);
                IEnumerable<mdl_Listado_Invoice> result = await factory.SQL.QueryAsync<mdl_Listado_Invoice>("EQUIP.fiscal.sp_Obtener_Listado_Invoice", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_MovimientoContable>> obtenerMovimientosContables(int batch, int invoice)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("batch", batch, System.Data.DbType.Int32);
                parametros.Add("idinvoice", invoice, System.Data.DbType.Int32);
                IEnumerable<mdl_Listado_MovimientoContable> result = await factory.SQL.QueryAsync<mdl_Listado_MovimientoContable>("EQUIP.fiscal.sp_Obtener_Listado_Movimientos_Contables", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> GuardarRelacion(mdl_Guardar_Relacion_InvoiceMovimiento mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    detalle = mdl.detalle
                };
                await factory.SQL.QueryAsync("EQUIP.fiscal.sp_Guardar_Relacion_Invoice_Movimientos", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
