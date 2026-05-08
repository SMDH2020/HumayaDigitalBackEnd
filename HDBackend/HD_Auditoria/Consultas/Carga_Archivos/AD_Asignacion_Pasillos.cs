using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Carga_Archivos;


namespace HD_Auditoria.Consultas.Carga_Archivos
{
    public class AD_Asignacion_Pasillos
    {
        private string CadenaConexion;
        public AD_Asignacion_Pasillos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Asignacion_Pasillos_Response> Asignacion_Pasillos(mdl_Asignacion_Pasillos mdl)
        {
            try
            {
                //Se construye un DataTable para pasarlo al TVP del stored
                var dt = new System.Data.DataTable();
                dt.Columns.Add("folio", typeof(string));
                dt.Columns.Add("pasillo", typeof(string));
                dt.Columns.Add("id_auditor", typeof(int));
                dt.Columns.Add("bloqueado_off", typeof(bool));

                foreach (var item in mdl.asignacion_pasillos)
                    dt.Rows.Add(item.folio, item.pasillo, item.id_auditor, item.bloqueado_off);

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@folio", mdl.folio, System.Data.DbType.String, System.Data.ParameterDirection.Input, 9);
                parametros.Add("@update_user", mdl.update_user, System.Data.DbType.Int16, System.Data.ParameterDirection.Input);

                //TVP: DataTable construido anteriormente
                parametros.Add("@asignaciones", dt.AsTableValuedParameter("Auditoria.TVP_ASIGNACION_PASILLOS"));

                //Parametros de respuesta
                parametros.Add("@resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@mensaje", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 200);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Auditoria.SP_INV_ASIGNAR_PASILLOS", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new mdl_Asignacion_Pasillos_Response
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
