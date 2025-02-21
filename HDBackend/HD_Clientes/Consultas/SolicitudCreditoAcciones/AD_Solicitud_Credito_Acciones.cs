using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.Solicitud_Credito_Acciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.SolicitudCreditoAcciones
{
    public class AD_Solicitud_Credito_Acciones
    {
        private string CadenaConexion;
        public AD_Solicitud_Credito_Acciones(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlSolicitudCredito_Enviar>> Guardar(mdlSolicitud_Credito_Acciones mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    accion = mdl.accion,
                    comentarios = mdl.comentarios,
                    usuario = mdl.usuario
                };
                IEnumerable<mdlSolicitudCredito_Enviar> result = await factory.SQL.QueryAsync<mdlSolicitudCredito_Enviar>("Credito.sp_Solicitud_Credito_Accion_Notificar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
