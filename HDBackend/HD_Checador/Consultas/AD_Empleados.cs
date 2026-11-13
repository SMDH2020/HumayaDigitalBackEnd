using Dapper;
using HD.AccesoDatos;
using HD_Checador.Modelos;

namespace HD_Checador.Consultas
{
    public class AD_Empleados
    {
        private readonly string CadenaConexion;

        public AD_Empleados(string cadenaConexion)
        {
            CadenaConexion = cadenaConexion;
        }

        public async Task<IEnumerable<mdl_Empleado>> ObtenerEmpleados()
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            var result = await factory.SQL.QueryAsync<mdl_Empleado>(
                "AppMH..sp_Obtener_Empleados",
                commandType: System.Data.CommandType.StoredProcedure);
            factory.SQL.Close();
            return result;
        }

        public async Task<mdl_AltasYBajas_View> ObtenerAltasYBajas(DateTime fechaInicio, DateTime fechaFin)
        {
            var parametros = new
            {
                fechainicio = fechaInicio.ToString("yyyy-MM-dd"),
                fechafin    = fechaFin.ToString("yyyy-MM-dd")
            };
            FactoryConection factory = new FactoryConection(CadenaConexion);
            var multi = await factory.SQL.QueryMultipleAsync(
                "AppMH.dbo.sp_OBtener_AltasYBajas",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure);

            var view = new mdl_AltasYBajas_View
            {
                altas = multi.Read<mdl_Empleado>().ToList(),
                bajas = multi.Read<mdl_Empleado>().ToList()
            };
            factory.SQL.Close();
            return view;
        }
    }
}
