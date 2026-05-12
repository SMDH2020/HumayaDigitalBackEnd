using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Conteo_Piezas;

namespace HD_Auditoria.Consultas.Conteo_Piezas
{
    public class AD_Listado_Inventario_Conteo_Piezas
    {
        private string CadenaConexion;
        public AD_Listado_Inventario_Conteo_Piezas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Listado_Inventario_Conteo_View> Inventario(string folio)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("folio", folio, System.Data.DbType.String);
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Auditoria.sp_Obtener_Listado_Inventario_Conteo_Folio", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Listado_Inventario_Conteo_View mdl = new mdl_Listado_Inventario_Conteo_View();
                mdl.header = result.Read<mdl_Listado_Inventario_Conteo_Header>().FirstOrDefault();
                mdl.listado_inv = result.Read<mdl_Listado_Inventario_Conteo_Piezas>().ToList();
                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
