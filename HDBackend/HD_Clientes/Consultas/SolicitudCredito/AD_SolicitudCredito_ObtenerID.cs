using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.SolicitudCredito
{
    public class AD_SolicitudCredito_ObtenerID
    {
        private string CadenaConexion;
        public AD_SolicitudCredito_ObtenerID(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSolicitud_Credito> ObteneriD(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                mdlSolicitud_Credito result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitud_Credito>("Credito.sp_solicitud_credito_ObtenerID", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
