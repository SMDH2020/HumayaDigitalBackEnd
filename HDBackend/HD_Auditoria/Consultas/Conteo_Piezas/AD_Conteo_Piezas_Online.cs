using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Carga_Archivos;
using HD_Auditoria.Modelos.Conteo_Piezas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Conteo_Piezas
{
    public class AD_Conteo_Piezas_Online
    {
        private string CadenaConexion;
        public AD_Conteo_Piezas_Online(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Conteo_Piezas_Online_Response> RegistrarConteo(mdl_Conteo_Piezas_Online mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@id_inv_fisico", mdl.id_inv_fisico, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parametros.Add("@id_auditor", mdl.id_auditor, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                parametros.Add("@conteo_fisico", mdl.conteo_fisico, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
                parametros.Add("@ubicacion_ok", mdl.ubicacion_ok, System.Data.DbType.Boolean, System.Data.ParameterDirection.Input);
                parametros.Add("@modo_captura", mdl.modo_captura, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parametros.Add("@id_sesion_off", mdl.id_sesion_off, System.Data.DbType.Int32);
                parametros.Add("@observacion", mdl.observacion, System.Data.DbType.String);

                parametros.Add("@id_conteo", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@diferencia", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                parametros.Add("@tipo_dif", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 10);
                parametros.Add("@resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@mensaje", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 200);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Auditoria.SP_CONTEO_REGISTRAR", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new mdl_Conteo_Piezas_Online_Response
                {
                    id_conteo = parametros.Get<int?>("@id_conteo") ?? 0,
                    diferencia = parametros.Get<float?>("@diferencia") ?? 0,
                    tipo_dif = parametros.Get<string?>("@tipo_dif"),
                    resultado = parametros.Get<int>("@resultado"),
                    mensaje = parametros.Get<string>("@mensaje")
                };
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> Justificar(mdl_Justificar_Piezas_Conteo mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@id_inv_fisico", mdl.id_inv_fisico, System.Data.DbType.Int32);
                parametros.Add("@justificadas", mdl.justificadas, System.Data.DbType.Single);
                parametros.Add("@justificacion", mdl.justificacion, System.Data.DbType.String);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync("Auditoria.sp_Justificar_Piezas_Conteo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Justificar_Piezas_Conteo> GetJustificacion(int id_inv_fisico)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@id_inv_fisico", id_inv_fisico, System.Data.DbType.Int32);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_Justificar_Piezas_Conteo result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Justificar_Piezas_Conteo>("Auditoria.sp_Obtener_Justificacion_Pieza_Conteo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
