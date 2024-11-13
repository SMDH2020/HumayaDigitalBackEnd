using Dapper;
using HD.AccesoDatos;
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
        public async Task<bool> Guardar(mdlSolicitud_Credito_Acciones mdl)
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
                await factory.SQL.QueryAsync<mdlSolicitud_Credito_Acciones>("Credito.sp_Solicitud_Credito_Accion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
