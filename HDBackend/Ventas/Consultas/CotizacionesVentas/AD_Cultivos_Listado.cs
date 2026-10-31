using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Consultas.Cultivos;
using HD_Ventas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Consultas.CotizacionesVentas
{
    public class AD_Cultivos_Listado
    {
        private string CadenaConexion;
        public AD_Cultivos_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Cultivos_Listado>> Listado(int adr)
        {
            try
            {
                var parametros = new
                {
                    adr = adr
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Cultivos_Listado> result = await factory.SQL.QueryAsync<mdl_Cultivos_Listado>("Ventas.sp_Cultivos_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
