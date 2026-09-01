using Dapper;
using HD_Finanzas.Modelos.Margenes;
using HD.AccesoDatos;

namespace HD_Finanzas.AccesoDatos
{
    public class FAD_Margenes
    {
        private string CadenaConexion;
        public FAD_Margenes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlMargenes>> GetMargenes(mdlERMargenes vm, string idusuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    ejercicio = vm.ejercicio,
                    periodo = vm.periodo,
                    adr = vm.adr,
                    sucursal = vm.sucursal,
                    departamento = vm.departamento,
                    idusuario = idusuario
                };
                var result = await factory.SQL.QueryAsync<mdlMargenes>("PixelCode.dbo.sp_GetMargenes", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<IEnumerable<mdlMargenes_Detalle_Completo>> GetMargenesDetalle(mdlMargenes_Detalle vm)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    fechainicio = vm.fechainicio,
                    fechafin = vm.fechafin,
                    adr = vm.adr,
                    sucursal = vm.sucursal,
                    familia = vm.familia,
                    usuario = vm.usuario
                };
                var result = await factory.SQL.QueryAsync<mdlMargenes_Detalle_Completo>("PixelCode.dbo.sp_GetMargenesDetalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
