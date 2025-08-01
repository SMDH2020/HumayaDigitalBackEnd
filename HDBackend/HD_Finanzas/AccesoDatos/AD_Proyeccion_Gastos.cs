using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.ProyeccionesGastos;
using HD_Finanzas.Modelos.ProyeccionesVentas;


namespace HD_Finanzas.AccesoDatos
{
    public class AD_Proyeccion_Gastos
    {
        private string CadenaConexion;
        public AD_Proyeccion_Gastos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Proyecciones_Gastos>> Obtener(mdl_Filtro_Proyecciones_Gastos vm, string usuario)
        {
            try
            {
                var parametros = new
                {
                    Ejercicio = vm.ejercicio,
                    Ejercicioant = vm.ejercicioant,
                    comparar = vm.comparar,
                    Periodos = vm.periodo,
                    Departamentos = vm.departamento,
                    Sucursales = vm.sucursal,
                    ADR = vm.adr,
                    Usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Proyecciones_Gastos> result = await factory.SQL.QueryAsync<mdl_Proyecciones_Gastos>("PixelCode.dbo.sp_Revision_ProyeccionGastos_HumayaDigital", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Poryeccion_Gasto_Anual>> ObtenerExcel(int ejercicio)
        {
            try
            {
                var parametros = new
                {
                  ejercicio = ejercicio,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Poryeccion_Gasto_Anual> result = await factory.SQL.QueryAsync<mdl_Poryeccion_Gasto_Anual>("PixelCode.dbo.sp_Obtener_Excel_ProyeccionGastos", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
