using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Clientes
{
    public class AD_Clientes_BuscarCoincidencias
    {
        private string CadenaConexion;
        public AD_Clientes_BuscarCoincidencias(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Clientes_Coincidencias>> Get(string cliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    cliente
                };
                IEnumerable<mdl_Clientes_Coincidencias> result = await factory.SQL.QueryAsync<mdl_Clientes_Coincidencias>("Credito.sp_Clientes_BuscarCoincidencia", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
