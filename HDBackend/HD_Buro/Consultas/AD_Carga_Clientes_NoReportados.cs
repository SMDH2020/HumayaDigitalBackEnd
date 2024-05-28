using Dapper;
using HD.AccesoDatos;
using HD_Buro.Modelos;

namespace HD_Buro.Consultas
{
    public class AD_Carga_Clientes_NoReportados
    {
        private string CadenaConexion;
        public AD_Carga_Clientes_NoReportados(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCarga_Clientes_NoReportados>> reporte(int ejercicio, int periodo, int sucursal, string mostrar)
        {
            try
            {
                var parametros = new
                {
                    Ejercicio = ejercicio,
                    Periodo = periodo,
                    Sucursal = sucursal,
                    Mostrar = mostrar
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Clientes_NoReportados> result = await factory.SQL.QueryAsync<mdlCarga_Clientes_NoReportados>("BuroCredito.dbo.sp_Carga_Clientes_NoReportados", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
