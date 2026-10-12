using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Justificaciones;
using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Justificaciones
{
    public class AD_JustificacionAuditoria_Timeline
    {
        private string CadenaConexion;
        public AD_JustificacionAuditoria_Timeline(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_TimelineView> Timeline(int idconteo)
        {
            try
            {

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@id_conteo", idconteo, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);


                FactoryConection factory = new FactoryConection(CadenaConexion);

                var result = await factory.SQL.QueryMultipleAsync("Auditoria.SP_JUST_AUDITORIA_TIMELINE", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_TimelineView listado = new mdl_TimelineView();
                listado.mensajes = result.Read<mdl_mensajes_Timeline>().ToList();
                listado.evidencia = result.Read<mdl_Evidencias_Timeline>().ToList();
                factory.SQL.Close();
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
