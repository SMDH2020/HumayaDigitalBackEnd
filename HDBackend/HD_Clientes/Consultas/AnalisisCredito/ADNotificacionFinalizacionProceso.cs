using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADNotificacionFinalizacionProceso
    {
        private string CadenaConexion;
        public ADNotificacionFinalizacionProceso(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task <IEnumerable<mdlCorreo_Notificacion>> GetBody(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                var result = await factory.SQL.QueryAsync<mdlCorreo_Notificacion>("Credito.sp_Solicitud_Credito_Email_Procesos", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
