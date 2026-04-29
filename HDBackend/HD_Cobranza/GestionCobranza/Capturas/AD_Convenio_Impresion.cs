using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Convenio_Impresion
    {
        private string CadenaConexion;
        public AD_Convenio_Impresion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Convenio_Impresion> Get(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                var result = await factory.SQL.QueryMultipleAsync("GestionCobranza.sp_Descargar_Convenio", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Convenio_Impresion impresion = new mdl_Convenio_Impresion();
                impresion.cliente = result.Read<mdl_Convenio_Guardar>().FirstOrDefault();
                impresion.facturas = result.Read<mdl_Facturas_Seleccionadas>().ToList();
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
