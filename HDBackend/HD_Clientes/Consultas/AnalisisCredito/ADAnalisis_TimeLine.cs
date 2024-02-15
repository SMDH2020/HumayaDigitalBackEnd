using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis;

namespace HD.Clientes.Consultas.AnalisisCredito
{
    public class ADAnalisis_TimeLine
    {
        private string CadenaConexion;
        public ADAnalisis_TimeLine(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlSCTimeline_View> BuscarFolio(string folio,string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio,
                    usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Timeline", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdlSCTimeline_View view = new mdlSCTimeline_View();
                view.estado = result.Read<mdlSCTimeline_estado>().FirstOrDefault();
                view.detalle = result.Read<mdlSCTimeline_detalle>().ToList();
                if (view.estado is null) view.estado = new mdlSCTimeline_estado();
                
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
