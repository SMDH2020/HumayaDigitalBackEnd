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
    public class AD_Listado_Clientes_RB
    {
        private string CadenaConexion;
        public AD_Listado_Clientes_RB(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_RB>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdl_Listado_RB>("Cobranza.sp_Clientes_RB_Listado", commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
