using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF
{
    public class ADJDF_Analisis_Documentacion_Listado
    {
        private string CadenaConexion;
        public ADJDF_Analisis_Documentacion_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCAnalisis_Documentacion_View> Get(string folio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Analisis_Documentacion_JDF", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSCAnalisis_Documentacion_View view = new mdlSCAnalisis_Documentacion_View();
                view.estado = result.Read<mdlSCAnalisis_Pedido_Estado>().FirstOrDefault();
                view.documentacion = result.Read<mdlSCAnalisis_Documentacion>().ToList();

                if (view.estado is null) view.estado = new mdlSCAnalisis_Pedido_Estado();

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
