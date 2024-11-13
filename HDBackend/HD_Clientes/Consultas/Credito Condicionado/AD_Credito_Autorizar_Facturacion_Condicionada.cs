using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;

namespace HD.Clientes.Consultas.Credito_Condicionado
{
    public class AD_Credito_Autorizar_Facturacion_Condicionada
    {
        private string CadenaConexion;
        FactoryConection factory;
        public AD_Credito_Autorizar_Facturacion_Condicionada(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
            factory = new FactoryConection(CadenaConexion);
        }
        public async Task<mdl_Analisis_100_view> Guardar(mdl_Autorizar_facturacion_View mdl)
        {
            var parametros = new
            {
                folio = mdl.folio,
                comentarios = mdl.comentarios,
                fecha = mdl.fecha,
                estatus=mdl.estatus,
                usuario = mdl.usuario,
            };
            var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Aprobar_Facturacion_Condicionada", parametros, commandType: System.Data.CommandType.StoredProcedure);
            mdl_Analisis_100_view view = new mdl_Analisis_100_view();
            view.encabezado = result.Read<mdl_Analisis_100_encabezado>().FirstOrDefault();
            view.detalle = result.Read<mdl_Analisis_100_detalle>().ToList();
            return view;
        }
    }
}
