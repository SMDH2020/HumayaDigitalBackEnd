using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;

namespace HD.Clientes.Consultas.Credito_Condicionado
{
    public class AD_Credito_Condicionado_Finanzas
    {
        private string CadenaConexion;
        FactoryConection factory;
        public AD_Credito_Condicionado_Finanzas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
            factory = new FactoryConection(CadenaConexion);
        }
        public async Task<mdl_Analisis_100_view> Obtener(string folio,string usuario)
        {
            try
            {
                var parametros = new
                {
                   folio,
                   usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.Credito_Condicionado_Analisis_Finanzas", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Analisis_100_view view = new mdl_Analisis_100_view();
                view.encabezado=result.Read<mdl_Analisis_100_encabezado>().FirstOrDefault();
                view.detalle = result.Read<mdl_Analisis_100_detalle>().ToList();
                return view;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
