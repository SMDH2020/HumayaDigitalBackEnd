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
    public class ADVencimientosSaldos
    {
        private string CadenaConexion;
        public ADVencimientosSaldos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlVencidosRevolvente>> ObtenerRevolvente(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };

                IEnumerable<mdlVencidosRevolvente> result = await factory.SQL.QueryAsync<mdlVencidosRevolvente>("Cobranza.Saldo_Vencido_Por_Cliente_Revolvente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlVencidosOperacion>> ObtenerOperacion(int idcliente)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente
                };

                IEnumerable<mdlVencidosOperacion> result = await factory.SQL.QueryAsync<mdlVencidosOperacion>("Cobranza.Saldo_Vencido_Por_Cliente_Operacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
