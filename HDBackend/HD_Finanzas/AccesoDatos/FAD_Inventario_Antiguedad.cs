using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.AntiguedadInventario;

namespace HD_Finanzas.AccesoDatos
{
    public class FAD_Inventario_Antiguedad
    {
        private string CadenaConexion;
        public FAD_Inventario_Antiguedad(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Inventario_Antiguedad_View> ObtenerInventario(mdl_vInventario vm)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ADR = vm.adr,
                    Sucursales = vm.sucursales,
                    Linea = vm.linea,
                    Antiguedad = vm.antiguedad,
                    Grupo = vm.grupo
                    //idusuario= idusuario
                };
                var result = await factory.SQL.QueryMultipleAsync("PixelCode.Inventario.SP_Obtener_Inventario", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var stockinfo = result.Read<mdl_Inventario_Antiguedad_Info>().FirstOrDefault();
                var stock = result.Read<mdl_Inventario_Antiguedad>().ToList();
                factory.SQL.Close();
                return new mdl_Inventario_Antiguedad_View
                {
                    InvAntiguedad = stock,
                    InvAntiguedadInfo = stockinfo
                };
            }
            catch (System.Exception ex)
            {

                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new
                {
                    errores = ex.Message
                });
            }
        }
    }
}
