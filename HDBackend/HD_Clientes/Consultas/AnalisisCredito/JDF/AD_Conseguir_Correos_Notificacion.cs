using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF
{
    public class AD_Conseguir_Correos_Notificacion
    {
        private string CadenaConexion;
        public AD_Conseguir_Correos_Notificacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable <mdlSolicitudCredito_Enviar>> ObtenerCorreos(string folio, string usuario, string comentarios)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario,
                    comentarios
                };
                IEnumerable<mdlSolicitudCredito_Enviar> result = await factory.SQL.QueryAsync<mdlSolicitudCredito_Enviar>("Credito.sp_Pedido_Detalle_Financiamiento_EQUIP_Guardar_Evento", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
