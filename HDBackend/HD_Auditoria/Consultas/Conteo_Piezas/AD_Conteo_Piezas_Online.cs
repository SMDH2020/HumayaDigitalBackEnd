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
                parametros.Add("@reg_contado", dbType: System.Data.DbType.Boolean, direction: ParameterDirection.Output);
                parametros.Add("@tipo_dif", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 10);
                parametros.Add("@resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@mensaje", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 200);

                ////KPIS
                //parametros.Add("@total_inventario_sku", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@registros_contados", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@registros_diferencias", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@registros_ubi_incorrecta", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@total_inventario_dinero", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@monto_total_diferencias", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@conf_loc", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@conf_inv", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@conf_mon", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@monto_total_inv", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@monto_total_faltante", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@porc_faltante", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@monto_total_sobrante", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@porc_sobrante", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@total_neto", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);
                //parametros.Add("@avance", dbType: System.Data.DbType.Single, direction: ParameterDirection.Output);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Auditoria.SP_CONTEO_REGISTRAR", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new mdl_Conteo_Piezas_Online_Response
                {
                    id_conteo = parametros.Get<int?>("@id_conteo") ?? 0,
                    diferencia = parametros.Get<float?>("@diferencia") ?? 0,
                    tipo_dif = parametros.Get<string?>("@tipo_dif"),
                    reg_contado = parametros.Get<bool>("@reg_contado"),
                    resultado = parametros.Get<int>("@resultado"),
                    mensaje = parametros.Get<string>("@mensaje"),

                    ////KPIS
                    //total_inventario_sku = parametros.Get<float?>("@total_inventario_sku") ?? 0,
                    //registros_contados = parametros.Get<float?>("@registros_contados") ?? 0,
                    //registros_diferencias = parametros.Get<float?>("@registros_diferencias") ?? 0,
                    //registros_ubi_incorrecta = parametros.Get<float?>("@registros_ubi_incorrecta") ?? 0,
                    //total_inventario_dinero = parametros.Get<float?>("@total_inventario_dinero") ?? 0,
                    //monto_total_diferencias = parametros.Get<float?>("@monto_total_diferencias") ?? 0,
                    //conf_loc = parametros.Get<float?>("@conf_loc") ?? 0,
                    //conf_inv = parametros.Get<float?>("@conf_inv") ?? 0,
                    //conf_mon = parametros.Get<float?>("@conf_mon") ?? 0,
                    //monto_total_inv = parametros.Get<float?>("@monto_total_inv") ?? 0,
                    //monto_total_faltante = parametros.Get<float?>("@monto_total_faltante") ?? 0,
                    //porc_faltante = parametros.Get<float?>("@porc_faltante") ?? 0,
                    //monto_total_sobrante = parametros.Get<float?>("@monto_total_sobrante") ?? 0,
                    //porc_sobrante = parametros.Get<float?>("@porc_sobrante") ?? 0,
                    //total_neto = parametros.Get<float?>("@total_neto") ?? 0,
                    //avance = parametros.Get<float?>("@avance") ?? 0
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

        public async Task<bool> Agregar_Posicion_Extra(mdl_Posicion_Extra mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@id_inv_fisico", mdl.id_inv_fisico, System.Data.DbType.Int32);
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String);
                parametros.Add("@posicion_extra", mdl.posicion_extra, System.Data.DbType.String);
                parametros.Add("@conteo_fisico", mdl.conteo_fisico, System.Data.DbType.Single);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync("Auditoria.sp_GUARDA_CONTEO_NUEVA_UBICACION", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> Eliminar_Posicion_Extra(mdl_Posicion_Extra mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@id_inv_fisico", mdl.id_inv_fisico, System.Data.DbType.Int32);
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String);
                parametros.Add("@posicion_extra", mdl.posicion_extra, System.Data.DbType.String);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync("Auditoria.sp_ELIMINA_CONTEO_NUEVA_UBICACION", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> AgregarNuevaPieza(mdl_Agregar_Nueva_Pieza mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String);
                parametros.Add("@familia", mdl.familia, System.Data.DbType.String);
                parametros.Add("@codigo", mdl.codigo, System.Data.DbType.String);
                parametros.Add("@descripcion", mdl.descripcion, System.Data.DbType.String);
                parametros.Add("@unidad_medida", mdl.unidad_medida, System.Data.DbType.String);
                parametros.Add("@costo_unitario", mdl.costo_unitario, System.Data.DbType.Single);
                parametros.Add("@conteo", mdl.conteo, System.Data.DbType.Single);
                parametros.Add("@posicion", mdl.posicion, System.Data.DbType.String);
                parametros.Add("@id_auditor", mdl.id_auditor, System.Data.DbType.Int16);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync("Auditoria.AGREGAR_NUEVA_PIEZA_INVENTARIO", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
