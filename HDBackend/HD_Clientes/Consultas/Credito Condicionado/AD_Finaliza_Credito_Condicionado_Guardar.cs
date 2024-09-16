using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.SC_Analisis.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.Credito_Condicionado
{
    public class AD_Finaliza_Credito_Condicionado_Guardar
    {
        private string CadenaConexion;
        public AD_Finaliza_Credito_Condicionado_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlAnalisis_Mhusa> Guardar(mdlSCAnalisis_Comentarios comentario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentario.folio,
                    idproceso = comentario.idproceso,
                    consecutivo = comentario.consecutivo,
                    comentarios = comentario.comentarios,
                    estatus = comentario.estatus,
                    usuario = comentario.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.SP_Finaliza_Credito_Condicionado_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlAnalisis_Mhusa mhusa = new mdlAnalisis_Mhusa();
                mhusa.mdldatos = result.Read<mdldatos_notificacion>().FirstOrDefault();
                mhusa.estado = result.Read<mdlSCAnalisis_Pedido_Estado>().FirstOrDefault();
                mhusa.mdlSolicitud = result.Read<mdlSolicitudCredito_Enviar>().ToList();
                factory.SQL.Close();
                return mhusa;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
