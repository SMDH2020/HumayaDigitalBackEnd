using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.Juridico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Consultas.Juridico
{
    public class AD_Clientes_Enviados_Juridico_Listado
    {
        private string CadenaConexion;
        public AD_Clientes_Enviados_Juridico_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Clientes_Juridico>> Listado(string adr, string sucursal, string estatus)
        {
            try
            {
                var parametros = new
                {
                    adr = adr,
                    sucursal = sucursal,
                    estatus = estatus
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Clientes_Juridico> result = await factory.SQL.QueryAsync<mdl_Clientes_Juridico>("Cartera_Clientes.Cobranza.sp_Clientes_Enviados_Juridico_Cobranza_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
