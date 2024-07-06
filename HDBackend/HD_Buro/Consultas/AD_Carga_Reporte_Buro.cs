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
        public async Task<IEnumerable<mdlCarga_Reporte_Buro>> reporte(mdlFiltrosView view)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = view.ejercicio,
                    periodo = view.periodo,
                    linea = view.linea,
                    vencimiento = view.vencimiento,
                    registrado = view.registrado,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Reporte_Buro> result = await factory.SQL.QueryAsync<mdlCarga_Reporte_Buro>("Cartera_clientes.dbo.sp_obtener_Listado_Mensual_Credito_Mhusa", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
