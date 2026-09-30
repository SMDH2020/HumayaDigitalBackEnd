using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF
{
    public class ADJDF_Analisis_AsignarPromotor
    {
        private string CadenaConexion;
        public ADJDF_Analisis_AsignarPromotor(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlJDFAnalisis_AsignarPromotorView> Get(string folio, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_JDF_Analisis_asignar_promotor", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlJDFAnalisis_AsignarPromotorView view = new mdlJDFAnalisis_AsignarPromotorView();
                view.estado = result.Read<mdlSCAnalisis_Pedido_Estado>().FirstOrDefault();
                view.promotor = result.Read<mdlJDFAnalisis_Asignarpromotor>().ToList();

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
