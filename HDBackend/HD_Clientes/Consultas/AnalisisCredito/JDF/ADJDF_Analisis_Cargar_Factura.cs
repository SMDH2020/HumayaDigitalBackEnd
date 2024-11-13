using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.Facturacion;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF
{
    public class ADJDF_Analisis_Cargar_Factura
    {
        private string CadenaConexion;
        public ADJDF_Analisis_Cargar_Factura(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlJDFAnalisis_Datos_Facturacion> Obtener(string folio,string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlJDFAnalisis_Datos_Facturacion>("Credito.sp_Analisis_JDF_Datos_Facturacion_View", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) { result = new mdlJDFAnalisis_Datos_Facturacion(); }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlJDFAnalisis_Datos_Facturacion> Guardar(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    registro = mdl.registro,
                    factura = mdl.factura,
                    nota_abono = mdl.nota_abono,
                    estatus = mdl.estatus,
                    idequip = mdl.idequip,
                    idsucursal = mdl.idsucursal,
                    serie_fiscal = mdl.serie_fiscal,
                    folio_fiscal = mdl.folio_fiscal,
                    documento = mdl.documento,
                    usuario = mdl.usuario
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlJDFAnalisis_Datos_Facturacion>("Credito.sp_Analisis_JDF_Datos_Facturacion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) { result = new mdlJDFAnalisis_Datos_Facturacion(); }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdlAnalisis_Mhusa> GuardarMhusa(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    registro=mdl.registro,
                    factura = mdl.factura,
                    nota_abono = mdl.nota_abono,
                    estatus = mdl.estatus,
                    idequip = mdl.idequip,
                    idsucursal = mdl.idsucursal,
                    serie_fiscal = mdl.serie_fiscal,
                    folio_fiscal = mdl.folio_fiscal,
                    documento = mdl.documento,
                    usuario = mdl.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Mhusa_Datos_Facturacion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlAnalisis_Mhusa mhusa = new mdlAnalisis_Mhusa();
                mhusa.mdldatos = result.Read<mdldatos_notificacion>().FirstOrDefault();
                mhusa.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();

                factory.SQL.Close();
    
                return mhusa;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdlJDFAnalisis_Datos_Facturacion> Guardar_detalle(string folio, int registro,int orden, string documento,string usuario, string docto_financiamiento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    registro,
                    orden,
                    documento,
                    usuario,
                    docto_financiamiento
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlJDFAnalisis_Datos_Facturacion>("Credito.sp_Pedido_Detalle_Financiamiento_EQUIP", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) { result = new mdlJDFAnalisis_Datos_Facturacion(); }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdlJDFAnalisis_Datos_Facturacion> Guardar_detalle_Condicionado(string folio, int registro, int orden, string documento, string usuario, string docto_financiamiento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    registro,
                    orden,
                    documento,
                    usuario,
                    docto_financiamiento
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlJDFAnalisis_Datos_Facturacion>("Credito.sp_Pedido_Detalle_Financiamiento_EQUIP_Condicionado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) { result = new mdlJDFAnalisis_Datos_Facturacion(); }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdlJDFAnalisis_Datos_Facturacion> Guardar_detalle_Condicionado_Mhusa(string folio, int registro, int orden, string documento, string usuario, string docto_financiamiento)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    registro,
                    orden,
                    documento,
                    usuario,
                    docto_financiamiento
                };
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlJDFAnalisis_Datos_Facturacion>("Credito.sp_Pedido_Detalle_Financiamiento_EQUIP_Condicionado_Mhusa", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) { result = new mdlJDFAnalisis_Datos_Facturacion(); }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
