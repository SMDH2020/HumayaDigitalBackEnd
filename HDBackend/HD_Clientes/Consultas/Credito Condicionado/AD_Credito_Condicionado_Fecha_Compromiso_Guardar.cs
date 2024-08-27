using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Clientes.Modelos.SC_Analisis.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Credito_Condicionado
{
    public class AD_Credito_Condicionado_Fecha_Compromiso_Guardar
    {
        private string CadenaConexion;
        public AD_Credito_Condicionado_Fecha_Compromiso_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSC_Credito_Condicionado> Guardar( string folio, string usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    folio=folio,
                    usuario=usuario,
                    fecha_compromiso=DateTime.Now,
                    comentarios= "Se agrego fecha compromiso"
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Documentacion_Fecha_Compromiso_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSC_Credito_Condicionado condicionado = new mdlSC_Credito_Condicionado();
                condicionado.datos_fecha = result.Read<mdl_fecha_compromiso>().FirstOrDefault();
                condicionado.mdldatos = result.Read<mdldatos_notificacion>().FirstOrDefault();
                condicionado.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();
                factory.SQL.Close();
                return condicionado;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
