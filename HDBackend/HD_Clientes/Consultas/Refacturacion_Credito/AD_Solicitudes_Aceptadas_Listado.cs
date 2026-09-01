using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Refacturacion_Credito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Refacturacion_Credito
{
    public class AD_Solicitudes_Aceptadas_Listado
    {

        private string CadenaConexion;
        public AD_Solicitudes_Aceptadas_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlSolicitudes_Aceptadas>> Listado()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlSolicitudes_Aceptadas> result = await factory.SQL.QueryAsync<mdlSolicitudes_Aceptadas>("Credito.sp_Listado_Solicitudes_Aceptadas", commandType: System.Data.CommandType.StoredProcedure);
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
