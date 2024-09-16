using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.ConvenioPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class ADConvenio_Pago_Impresion
    {
        private string CadenaConexion;
        public ADConvenio_Pago_Impresion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlConvenio_Pago_Impresion> Get(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                var result = await factory.SQL.QueryMultipleAsync("Cobranza.sp_Convenio_Pago_PDF", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlConvenio_Pago_Impresion impresion = new mdlConvenio_Pago_Impresion();
                impresion.cliente = result.Read<mdlConvenio_Pago>().FirstOrDefault();
                impresion.facturas = result.Read<mdlFacturasSeleccionadas>().ToList();
                factory.SQL.Close();
                return impresion;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
