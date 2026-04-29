using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturar_Equipo;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Clientes.Modelos.SC_Analisis.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF_Condicionado
{
    public class ADFacturacion_Equipo_JDF
    {
        private string CadenaConexion;
        public ADFacturacion_Equipo_JDF(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
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
                var result = await factory.SQL.QueryFirstOrDefaultAsync<mdlJDFAnalisis_Datos_Facturacion>("Credito.sp_SCJ_Facturacion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                if (result == null) { result = new mdlJDFAnalisis_Datos_Facturacion(); }
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_datos_view> informacion(string folio, int usuario)
        {
            try
            {
                var parametros = new
                {
                    folio,
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_JDF_Datos_Facturacion_View_JDF", parametros, commandType: System.Data.CommandType.StoredProcedure);

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
