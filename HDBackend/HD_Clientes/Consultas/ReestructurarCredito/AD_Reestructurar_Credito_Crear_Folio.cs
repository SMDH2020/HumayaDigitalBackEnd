using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.ReestructurarCredito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.ReestructurarCredito
{
    public class AD_Reestructurar_Credito_Crear_Folio
    {
        private string CadenaConexion;
        public AD_Reestructurar_Credito_Crear_Folio(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlPedido_Condiciones_Venta> Crear(string folio, int usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                mdlPedido_Condiciones_Venta result = await factory.SQL.QueryFirstOrDefaultAsync<mdlPedido_Condiciones_Venta>("Credito.sp_Crear_Solicitud_Reestructuracion_Credito", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
