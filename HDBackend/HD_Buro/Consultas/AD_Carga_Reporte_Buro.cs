using Dapper;
using HD.AccesoDatos;
using HD_Buro.Modelos;

namespace HD_Buro.Consultas
{
    public class AD_Carga_Reporte_Buro
    {
        private string CadenaConexion;
        public AD_Carga_Reporte_Buro(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCarga_Reporte_Buro>> reporte(int ejercicio, int periodo, int sucursal, string mostrar)
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
                IEnumerable<mdlCarga_Reporte_Buro> result = await factory.SQL.QueryAsync<mdlCarga_Reporte_Buro>("BuroCredito.dbo.sp_Clientes_Cierre_Mensual", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
