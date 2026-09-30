using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.ReferenciasBancarias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas.ReferenciasBancarias
{
    public class AD_Clientes_RB_Obtener_Referencia
    {
        private string CadenaConexion;
        public AD_Clientes_RB_Obtener_Referencia(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Reporte_RB> obtenerReferencia(string idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente= idcliente,
                };
                mdl_Reporte_RB result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Reporte_RB>("Cobranza.sp_Clientes_RB_Obtener_Referencia", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
