using Dapper;
using HD.AccesoDatos;
using HD_Ventas;
using HD_Ventas.Modelos.EmbudoVentas;

namespace HD_Ventas.Consultas
{
    public class AD_Embudo_Ventas
    {
        private string CadenaConexion;
        public AD_Embudo_Ventas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Embudo_Ventas> ObtenerEmbudo(string fecha_inicio, string fecha_fin, int esquema, string fase, int usuario, int linea, string cultivo)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("Fecha_inicio", fecha_inicio, System.Data.DbType.String);
                parametros.Add("Fecha_fin", fecha_fin, System.Data.DbType.String);
                parametros.Add("esquema", esquema, System.Data.DbType.Int16);
                parametros.Add("fase", fase, System.Data.DbType.String);
                parametros.Add("usuario", usuario, System.Data.DbType.Int16);
                parametros.Add("linea", linea, System.Data.DbType.Int16);
                parametros.Add("cultivo", cultivo, System.Data.DbType.String);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("ventas.sp_Obtener_Reporte_Embudo_Ventas", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Embudo_Ventas mdl = new mdl_Embudo_Ventas();
                mdl.permiso = result.Read<mdl_Embudo_Ventas_Permisos>().FirstOrDefault();
                mdl.data = result.Read<mdl_Embudo_Ventas_Data>().ToList();
                mdl.regiones = result.Read<mdl_Embudo_Ventas_Regiones>().ToList();
                mdl.sucursales = result.Read<mdl_Embudo_Ventas_Sucursales>().ToList();
                mdl.lineas = result.Read<mdl_Embudo_Ventas_Lineas>().ToList();
                mdl.departamentos = result.Read<mdl_Embudo_Ventas_Departamentos>().ToList();
                mdl.asesores = result.Read<mdl_Embudo_Ventas_Asesores>().ToList();

                factory.SQL.Close();
                return mdl;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Embudo_Ventas_DDLLinea>> DDLLineas()
        {
            try
            {
                var parametros = new
                {
                    //usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Embudo_Ventas_DDLLinea> result = await factory.SQL.QueryAsync<mdl_Embudo_Ventas_DDLLinea>("Ventas.sp_Get_Lineas_Embudo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
