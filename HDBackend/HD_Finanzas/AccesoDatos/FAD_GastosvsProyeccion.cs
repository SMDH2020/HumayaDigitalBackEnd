using Dapper;
using HD.AccesoDatos;
using HD_Finanzas.Modelos.Gastos_Proyeccion;

namespace HD_Finanzas.AccesoDatos
{
    public class FAD_GastosvsProyeccion
    {
        private string CadenaConexion;
        public FAD_GastosvsProyeccion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<Fmdl_GastosPorConcepto>> GetGastosvsProyeccion(Fmdl_Gastos_Filtros vm, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    fechainicio = vm.fechainicio,
                    fechafin = vm.fechafin,
                    adr = vm.adr,
                    sucursal = vm.sucursales,
                    departamento = vm.departamento,
                    tipo = vm.tipo,
                    usuario = usuario
                };
                IEnumerable<Fmdl_GastosPorConcepto> gastosvs = await factory.SQL.QueryAsync<Fmdl_GastosPorConcepto>("PixelCode.dbo.SP_GastosvsProyeccion_Rolado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return gastosvs;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
