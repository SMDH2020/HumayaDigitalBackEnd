using CarteraTransito.Modelos;
using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace CarteraTransito.Consultas
{
    public class AD_Cartera_Transito_Listado
    {
        private string CadenaConexion;
        public AD_Cartera_Transito_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Cartera_Transito>> Listado(int ejercicio, int periodo, string? sucursal, string? adr, string pendientes)
        {
            try
            {
                var parametros = new
                {
                    @ejercicio = ejercicio,
                    @periodo = periodo,
                    sucursal = sucursal,
                    adr = adr,
                    @pendientes = pendientes
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Cartera_Transito> result = await factory.SQL.QueryAsync<mdl_Cartera_Transito>("Cartera_Clientes.dbo.sp_Cartera_Transito_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdlDetalleISC> ObtenerResumen(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = folio,
                };
                var result = await factory.SQL.QueryMultipleAsync("Cartera_Clientes.dbo.Get_Detalle_ISC", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdlDetalleISC view = new mdlDetalleISC();
                view.resumen_facturacion = result.Read<mdl_Tabla_Diferencias_Facturacion>().ToList();
                view.documentos_faltantes = result.Read<mdl_Tabla_Documentos_Faltantes>().ToList();

                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
