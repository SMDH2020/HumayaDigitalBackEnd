using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturar_Equipo;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.Facturar_Equipo
{
    public class AD_Facturar_Equipo_Estado
    {
        private string CadenaConexion;
        public AD_Facturar_Equipo_Estado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_datos_view> informacion(string folio,int usuario)
        {
            try
            {
                var parametros = new
                { 
                    folio,
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_JDF_Datos_Facturacion_View", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdl_datos_view data = new mdl_datos_view();
                data.datos_pedido = result.Read<mdl_datos_pedido>().FirstOrDefault();
                data.informacion = result.Read<mdlSCAnalisis_Pedido_Estado>().FirstOrDefault();
                data.unidades = result.Read<mdlFacturaUnidadesSolicitadas>().ToList();
                data.financiamiento = result.Read<mdlPEdidoFinanciamiento>().ToList();
                factory.SQL.Close();

                return data;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdl_datos_factura_unidad> informacion(string folio,int registro, int usuario)
        {
            try
            {
                var parametros = new
                {
                    folio,
                    registro,
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Obtener_Datos_Fatura_Por_Unidad", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdl_datos_factura_unidad data = new mdl_datos_factura_unidad();
                data.datos_pedido = result.Read<mdl_comentarios>().FirstOrDefault();
                data.sucursales = result.Read<mdl_sucursales_cliente>().ToList();
                data.documentos = result.Read<mdl_documentos_facturados_EQUIP>().ToList();
                factory.SQL.Close();

                return data;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<bool> EliminarRegistro(string folio, int registro, int orden)
        {
            try
            {
                var parametros = new
                {
                    folio,
                    registro,
                    orden
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                 await factory.SQL.QueryAsync("Credito.sp_Solicitud_Credito_Documento_Factura_Eliminar", parametros, commandType: System.Data.CommandType.StoredProcedure);

               
                factory.SQL.Close();

                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<mdl_datos_view> Paricionar(string folio,int registro, int usuario)
        {
            try
            {
                var parametros = new
                {
                    folio,
                    registro,
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Pedido_Unidades_Particionar", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdl_datos_view data = new mdl_datos_view();
                data.datos_pedido = result.Read<mdl_datos_pedido>().FirstOrDefault();
                data.informacion = result.Read<mdlSCAnalisis_Pedido_Estado>().FirstOrDefault();
                data.unidades = result.Read<mdlFacturaUnidadesSolicitadas>().ToList();
                data.financiamiento = result.Read<mdlPEdidoFinanciamiento>().ToList();
                factory.SQL.Close();

                return data;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
