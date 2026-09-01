using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.Actions;

namespace HD_Finanzas.AccesoDatos.Actions
{
    public class FAD_AdrSucursalDepto
    {
        private string CadenaConexion;
        public FAD_AdrSucursalDepto(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<FmdlADRScucursalDep>> GetASD(string IdUsuario, string Tipo = "G")
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    IdUsuario = IdUsuario,
                    Tipo = Tipo
                };

                IEnumerable<FmdlADRScucursalDep> asd = await factory.SQL.QueryAsync<FmdlADRScucursalDep>("PixelCode.dbo.SP_Get_ADR_SUCURSAL_DEPARTAMENTO", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return asd;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }

        public async Task<Fmdl_ADRSucursal_Ejercicio_View> GetASDCXC(string IdUsuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    IdUsuario = IdUsuario
                    //Tipo = Tipo
                };

                var result = await factory.SQL.QueryMultipleAsync("PixelCode.dbo.SP_Get_ADR_SUCURSAL_DEPARTAMENTO_CXC", parametros, commandType: System.Data.CommandType.StoredProcedure);
                Fmdl_ADRSucursal_Ejercicio_View mhusa = new Fmdl_ADRSucursal_Ejercicio_View();
                mhusa.filtro = result.Read<FmdlADRScucursalDep>().ToList();
                mhusa.fechas = result.Read<Fmdl_Ejercicios_Conciliaciones>().ToList();

                factory.SQL.Close();
                return mhusa;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }

        public async Task<Fmdl_ADRSucursal_Ejercicio_View_CXC> GetASDCXC2(string IdUsuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    IdUsuario = IdUsuario
                    //Tipo = Tipo
                };

                var result = await factory.SQL.QueryMultipleAsync("PixelCode.dbo.SP_Get_Control_Filtro_CXC", parametros, commandType: System.Data.CommandType.StoredProcedure);
                Fmdl_ADRSucursal_Ejercicio_View_CXC mhusa = new Fmdl_ADRSucursal_Ejercicio_View_CXC();
                mhusa.filtro = result.Read<FmdlADRSucursalDepCXC>().ToList();
                mhusa.fechas = result.Read<Fmdl_Ejercicios_Conciliaciones>().ToList();

                factory.SQL.Close();
                return mhusa;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }
    }
}
