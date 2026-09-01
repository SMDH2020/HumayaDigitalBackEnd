using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.Juridico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas.Juridico
{
    public class AD_Timeline_Eventos_Obtener
    {
        private string CadenaConexion;
        public AD_Timeline_Eventos_Obtener(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Timeline_Eventos>> Timeline(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente = idcliente,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Timeline_Eventos> result = await factory.SQL.QueryAsync<mdl_Timeline_Eventos>("Cobranza.sp_Eventos_Timeline", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
