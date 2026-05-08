using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Carga_Archivos;

namespace HD_Auditoria.Consultas.Carga_Archivos
{
    public class AD_Carga_Inventario_Ajustes
    {
        private string CadenaConexion;
        public AD_Carga_Inventario_Ajustes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Cargar_Inventario_Ajustes_Response> Carga_Inventario_Ajustes(mdl_Cargar_Inventario_Ajustes mdl)
        {
            try
            {
                //Se construye un DataTable para pasarlo al TVP del stored
                var dt = new System.Data.DataTable();
                dt.Columns.Add("codigo", typeof(string));
                dt.Columns.Add("descripcion", typeof(string));
                dt.Columns.Add("cantidad", typeof(float));
                dt.Columns.Add("sucursal_origen", typeof(string));
                dt.Columns.Add("sucursal_dest", typeof(string));
                dt.Columns.Add("fecha_envio", typeof(string));
                dt.Columns.Add("referencia_doc", typeof(string));

                foreach (var item in mdl.ajustes)
                    dt.Rows.Add(item.codigo, item.descripcion, item.cantidad, item.sucursal_origen, item.sucursal_dest, item.fecha_envio, item.referencia_doc);

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String, System.Data.ParameterDirection.Input, 9);
                parametros.Add("@tipo_ajuste", mdl.tipo_ajuste, System.Data.DbType.String, System.Data.ParameterDirection.Input, 1);
                parametros.Add("@id_usuario", mdl.id_usuario, System.Data.DbType.Int16, System.Data.ParameterDirection.Input);

                //TVP: DataTable construido anteriormente
                parametros.Add("@ajustes", dt.AsTableValuedParameter("Auditoria.TVP_AJUSTES"));

                //Parametros de respuesta
                parametros.Add("@total", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@ok", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@errores", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@mensaje", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 200);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Auditoria.SP_INV_CARGAR_AJUSTES", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new mdl_Cargar_Inventario_Ajustes_Response
                {
                    Total = parametros.Get<int>("@total"),
                    Ok = parametros.Get<int>("@ok"),
                    Errores = parametros.Get<int>("@errores"),
                    Resultado = parametros.Get<int>("@resultado"),
                    Mensaje = parametros.Get<string>("@mensaje")
                };
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
