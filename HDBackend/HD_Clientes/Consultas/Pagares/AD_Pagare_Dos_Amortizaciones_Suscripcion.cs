using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Pagares;
using HD.Clientes.Modelos.Pedido_Impresion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Pagares
{
    public class AD_Pagare_Dos_Amortizaciones_Suscripcion
    {
        private string CadenaConexion;
        public AD_Pagare_Dos_Amortizaciones_Suscripcion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Pagare_Impresion> Get(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Pagare_Impresion_PDF", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Pagare_Impresion impresion = new mdl_Pagare_Impresion();
                impresion.ubicacion = result.Read<mdl_Pagare_Ubicacion_View>().FirstOrDefault();
                impresion.financiamiento = result.Read<mdl_Pedido_Financiamiento_View>().ToList();
                impresion.firmas = result.Read<mdl_Pedido_Firmas_View>().FirstOrDefault();
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
