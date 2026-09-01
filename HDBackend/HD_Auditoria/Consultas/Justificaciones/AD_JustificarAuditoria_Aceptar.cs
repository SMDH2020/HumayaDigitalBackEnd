using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Justificaciones;
using HD_Auditoria.Modelos.Programar_Inventario;

namespace HD_Auditoria.Consultas.Justificaciones
{
    public class AD_JustificarAuditoria_Aceptar
    {
        private string CadenaConexion;
        public AD_JustificarAuditoria_Aceptar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Result_SP> JustificacionAceptada(mdl_Justificaciones_Acciones mdl)
        {
            try
            {

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String, System.Data.ParameterDirection.Input, 9);
                parametros.Add("@id_just", mdl.idjust, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                parametros.Add("@tipo_aceptacion", mdl.tipo_aceptacion, System.Data.DbType.String, System.Data.ParameterDirection.Input,20);
                parametros.Add("@cantidad_aceptada", mdl.cantidad_aceptada, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                parametros.Add("@usuario", mdl.usuario, System.Data.DbType.String, System.Data.ParameterDirection.Input);

                //Parametros de respuesta
                parametros.Add("@resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@mensaje", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 500);
                parametros.Add("@completado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Auditoria.SP_JUST_AUDITORIA_ACEPTAR", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new mdl_Result_SP
                {
                    resultado = parametros.Get<int>("@resultado"),
                    mensaje = parametros.Get<string>("@mensaje")
                };

            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
