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
    public class AD_Justificar_Auditoria_Responsable_Almacen_Guardar
    {
        private string CadenaConexion;
        public AD_Justificar_Auditoria_Responsable_Almacen_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Result_SP> GuardarJustificacion(mdl_Justificaciones_Guardar mdl, string? jsonMetadata)
        {
            try
            {

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String, System.Data.ParameterDirection.Input,9);
                parametros.Add("@id_just", mdl.idjust, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                parametros.Add("@id_conteo", mdl.idconteo, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                parametros.Add("@comentario", mdl.comentario, System.Data.DbType.String, System.Data.ParameterDirection.Input,500);
                parametros.Add("@archivos", jsonMetadata, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parametros.Add("@usuario", mdl.usuario, System.Data.DbType.String, System.Data.ParameterDirection.Input);


                //Parametros de respuesta
                parametros.Add("@resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@mensaje", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 500);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Auditoria.SP_JUST_AUDITORIA_RES_ALMACEN_GUARDAR", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
