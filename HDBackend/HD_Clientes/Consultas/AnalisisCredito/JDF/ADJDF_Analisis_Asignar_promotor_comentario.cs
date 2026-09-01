using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Clientes.Modelos.SC_Analisis.Modal;

namespace HD.Clientes.Consultas.AnalisisCredito.JDF
{
    public class ADJDF_Analisis_Asignar_promotor_comentario
    {
        private string CadenaConexion;
        public ADJDF_Analisis_Asignar_promotor_comentario(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlJDFAnalisis_AsignarPromotorView> Get(mdlJDFAnalisis_Asignar_Promotor_Comentarios comentarios)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = comentarios.folio,
                    idpromotor= comentarios.idpromotor,
                    comentarios = comentarios.comentarios,
                    usuario = comentarios.usuario
                };
                var view = await factory.SQL.QueryMultipleAsync("Credito.sp_AC_asignar_promotor_comentario", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlJDFAnalisis_AsignarPromotorView result = new mdlJDFAnalisis_AsignarPromotorView();
                result.estado = view.Read<mdlSCAnalisis_Pedido_Estado>().FirstOrDefault();
                result.promotor = view.Read<mdlJDFAnalisis_Asignarpromotor>().ToList();

                if (result.estado is null) result.estado = new mdlSCAnalisis_Pedido_Estado();

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
