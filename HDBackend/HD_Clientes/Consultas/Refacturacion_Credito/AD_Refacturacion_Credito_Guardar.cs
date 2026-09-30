using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Refacturacion_Credito
{
    public class AD_Refacturacion_Credito_Guardar
    {
        private string CadenaConexion;
        public AD_Refacturacion_Credito_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdlJDFAnalisis_Datos_Facturacion_Notificacion_View> Guardar(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
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
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Refacturar_Credito_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlJDFAnalisis_Datos_Facturacion_Notificacion_View mhusa = new mdlJDFAnalisis_Datos_Facturacion_Notificacion_View();
                mhusa.datos_facturacion = result.Read<mdlJDFAnalisis_Datos_Facturacion>().FirstOrDefault();
                mhusa.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();

                factory.SQL.Close();

                return mhusa;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
