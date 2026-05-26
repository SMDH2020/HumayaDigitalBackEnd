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
    public class AD_ExtenderFechaJustificacion_Guardar
    {
        private string CadenaConexion;
        public AD_ExtenderFechaJustificacion_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Notificar_View> ExtenderFecha(mdl_ExtenderFecha mdl)
        {
            try
            {

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String, System.Data.ParameterDirection.Input, 9);
                parametros.Add("@fecha_fin", mdl.fecha_fin, System.Data.DbType.String, System.Data.ParameterDirection.Input, 10);
                parametros.Add("@usuario", mdl.usuario, System.Data.DbType.String, System.Data.ParameterDirection.Input);

                //Parametros de respuesta
                parametros.Add("@resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@mensaje", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 500);
                parametros.Add("@finalizado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Auditoria.SP_JUST_AUDITORIA_EXTENDER_FECHA_JUSTIFICACION_GUARDAR", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Notificar_View listado = new mdl_Notificar_View();
                listado.correos = result.Read<mdl_Notificar_Correo>().ToList();
                listado.estatus = new mdl_Result_SP
                {
                    resultado = parametros.Get<int>("@resultado"),
                    mensaje = parametros.Get<string>("@mensaje"),
                    finalizado = parametros.Get<int>("@finalizado")
                };
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
