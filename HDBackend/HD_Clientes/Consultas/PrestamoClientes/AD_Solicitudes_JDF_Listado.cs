using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.PrestamoClientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.PrestamoClientes
{
    public class AD_Solicitudes_JDF_Listado
    {

        private string CadenaConexion;
        public AD_Solicitudes_JDF_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlPrestamo_Clientes>> Get()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlPrestamo_Clientes> result = await factory.SQL.QueryAsync<mdlPrestamo_Clientes>("Credito.sp_Listado_Solicitudes_JDF", commandType: System.Data.CommandType.StoredProcedure);
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
