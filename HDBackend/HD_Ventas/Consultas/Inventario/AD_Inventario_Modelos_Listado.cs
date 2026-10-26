using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.Inventario;

namespace HD_Ventas.Consultas.Inventario
{
    public class AD_Inventario_Modelos_Listado
    {
        private string CadenaConexion;
        public AD_Inventario_Modelos_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Inventario_Modelos_Listado_View> Listado()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Ventas.sp_Inventario_Modelos_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Inventario_Modelos_Listado_View mdl = new mdl_Inventario_Modelos_Listado_View();
                mdl.inventario = result.Read<mdl_Inventario_Modelos_Listado>().ToList();
                mdl.departamentos = result.Read<mdl_Inventario_Departamentos>().ToList();
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
