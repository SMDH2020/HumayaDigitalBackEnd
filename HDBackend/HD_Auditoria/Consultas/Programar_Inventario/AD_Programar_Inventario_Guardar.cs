using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Carga_Archivos;
using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Programar_Inventario
{
    public class AD_Programar_Inventario_Guardar
    {
        private string CadenaConexion;
        public AD_Programar_Inventario_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Result_SP> ProgramarInventario(mdl_Programar_Inventario mdl)
        {
            try
            {

                //Parametros de entrada
                var parametros = new DynamicParameters();
                parametros.Add("@id_sucursal", mdl.id_sucursal, System.Data.DbType.Int16, System.Data.ParameterDirection.Input);
                parametros.Add("@tipo_inventario", mdl.tipo_inventario, System.Data.DbType.String, System.Data.ParameterDirection.Input, 1);
                parametros.Add("@fecha_inicio", mdl.fecha_inicio, System.Data.DbType.String, System.Data.ParameterDirection.Input, 10);
                parametros.Add("@fecha_fin", mdl.fecha_fin, System.Data.DbType.String, System.Data.ParameterDirection.Input, 10);
                parametros.Add("@fecha_limite_just", mdl.fecha_limite_just, System.Data.DbType.String, System.Data.ParameterDirection.Input, 10);
                parametros.Add("@id_encargado_alm", mdl.id_encargado_alm, System.Data.DbType.Int16, System.Data.ParameterDirection.Input);
                parametros.Add("@id_auditor_ppal", mdl.id_auditor_ppal, System.Data.DbType.Int16, System.Data.ParameterDirection.Input);
                parametros.Add("@observaciones", mdl.observaciones, System.Data.DbType.String, System.Data.ParameterDirection.Input, 500);
                parametros.Add("@auditores_adicionales", mdl.auditores_adicionales, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parametros.Add("@categorias", mdl.categorias, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                parametros.Add("@usuario", mdl.usuario, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                //parametros.Add("@folio", "000000000", System.Data.DbType.String, System.Data.ParameterDirection.Input);

                //Parametros de respuesta
                parametros.Add("@folio", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 9);
                parametros.Add("@resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                parametros.Add("@mensaje", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 500);


                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Auditoria.SP_PROG_AUDITORIA_GUARDAR", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new mdl_Result_SP
                {
                    folio = parametros.Get<string>("@folio"),
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
