using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos.ReportePrestamosClientes;

namespace HD_Cobranza.Capturas.ReportePrestamosClientes
{
    public class AD_ReportePrestamosClientes
    {
        private string CadenaConexion;
        public AD_ReportePrestamosClientes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlReportePrestamosClientes>> Listado(string? origen_registro, DateTime? fecha_desde, DateTime? fecha_hasta, string? ADR, string? sucursal)
        {
            try
            {
                // Lo que no se envia viaja como NULL, nunca como cadena vacia:
                // el SP no aplica ese filtro cuando recibe NULL.
                string? origen = string.IsNullOrWhiteSpace(origen_registro) ? null : origen_registro.Trim();

                var parametros = new
                {
                    @origen_registro = origen,
                    @fecha_desde = fecha_desde,
                    @fecha_hasta = fecha_hasta,
                    @ADR         = ADR,
                    @sucursal    = sucursal
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlReportePrestamosClientes> result = await factory.SQL.QueryAsync<mdlReportePrestamosClientes>("Credito.sp_Reporte_Prestamos_Clientes", parametros, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 180);
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
