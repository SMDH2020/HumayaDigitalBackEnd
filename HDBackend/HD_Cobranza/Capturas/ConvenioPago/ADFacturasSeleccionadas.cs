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
    public class ADFacturasSeleccionadas
    {
        private string CadenaConexion;
        public ADFacturasSeleccionadas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlFacturasSeleccionadas>> ObtenerOperacion(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };

                IEnumerable<mdlFacturasSeleccionadas> result = await factory.SQL.QueryAsync<mdlFacturasSeleccionadas>("Cobranza.Saldo_Vencido_Por_Cliente_Operacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
