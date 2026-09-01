using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.ConvenioPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas.ConvenioPago
{
    public class ADCPClientesPredictivo
    {
        private string CadenaConexion;
        public ADCPClientesPredictivo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCPClientesPredictivo>> Listado(string idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };

                IEnumerable<mdlCPClientesPredictivo> result = await factory.SQL.QueryAsync<mdlCPClientesPredictivo>("Cobranza.SP_Obtener_Clientes_Registrados", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
