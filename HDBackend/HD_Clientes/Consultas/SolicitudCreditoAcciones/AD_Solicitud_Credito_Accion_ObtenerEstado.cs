using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Modal;
using HD.Clientes.Modelos.Solicitud_Credito_Acciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.SolicitudCreditoAcciones
{
    public class AD_Solicitud_Credito_Accion_ObtenerEstado
    {
        private string CadenaConexion;
        public AD_Solicitud_Credito_Accion_ObtenerEstado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSolicitud_Credito_Acciones> Obtener(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                mdlSolicitud_Credito_Acciones result = await factory.SQL.QueryFirstOrDefaultAsync<mdlSolicitud_Credito_Acciones>("Credito.sp_Solicitud_Credito_Accion_Obtener_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
