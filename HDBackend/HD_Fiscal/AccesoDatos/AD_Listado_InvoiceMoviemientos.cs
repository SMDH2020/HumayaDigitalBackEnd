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
        public async Task<mdlObtenerXml> ObtenerXML (string document_no)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("documento", document_no);

                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlObtenerXml>("EQUIP.fiscal.sp_obtener_xml", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                result = result is null ? new mdlObtenerXml() : result;
                return result;
            }
            catch (Exception ex)
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
                view.Descuentos_Timbrados_Ventas = result.Read<mdl_Listado_Incidencia_Descuentos_Timbrados_ComoVentas>().ToList();
                view.Facturacion_NoRegistrada = result.Read<mdl_Listado_Incidencia_Facturacion_NoRegistrada_EnVentas>().ToList();
                view.Facturación_SinUuid = result.Read<mdl_Listado_Incidencia_Facturacion_SinUuid>().ToList();
                view.Reversas = result.Read<mdl_Reversas_UUID_Vigente>().ToList();
                view.CancelacionesSat_VigentesEQUIP = result.Read<mdl_Listado_Incidencias_CancelacionesSAT_VigentesEQUIP>().ToList();
                view.Reversas_Pendientes_Aplicar = result.Read<mdl_Listado_Incidencias_Reversas_Pendientes_Aplicar>().ToList();
                view.Candidatos_Refacturacion = result.Read<mdl_Listado_Incidencias_Candidatos_Refacturacion>().ToList();
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

        public async Task<IEnumerable<mdl_Detalle_Candidatos_Refacturacion>> obtenerDetalleCandidatos(int document_no, string von_no)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("document_no", document_no, System.Data.DbType.Int32);
                parametros.Add("von_no", von_no, System.Data.DbType.String);
                IEnumerable<mdl_Detalle_Candidatos_Refacturacion> result = await factory.SQL.QueryAsync<mdl_Detalle_Candidatos_Refacturacion>("EQUIP.fiscal.sp_Obtener_Posibles_Refacturaciones_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Buscar_Documento_Invoice>> buscarDocumento(int documento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new DynamicParameters();
                parametros.Add("documento", documento, System.Data.DbType.Int32);
                IEnumerable<mdl_Buscar_Documento_Invoice> result = await factory.SQL.QueryAsync<mdl_Buscar_Documento_Invoice>("EQUIP.fiscal.sp_Buscar_Documento_Invoice", parametros, commandType: System.Data.CommandType.StoredProcedure);
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

        public async Task<bool> AplicarReversa(mdl_Aplicar_Reversa mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    document_cancelacion = mdl.document_cancelacion,
                    document_orig = mdl.document_orig,
                    document_refacturacion = mdl.document_refacturacion
                };
                await factory.SQL.QueryAsync("EQUIP.fiscal.sp_Aplicar_reversa", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> AplicarRefacturacion(mdl_Aplicar_Refacturacion_Documento mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    document_candidato = mdl.document_candidato,
                    document_refacturacion = mdl.document_refacturacion,
                    document_reversa = mdl.document_reversa
                };
                await factory.SQL.QueryAsync("EQUIP.fiscal.sp_Aplicar_Documento_Refacturacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
