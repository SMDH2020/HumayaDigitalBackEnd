using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturar_Equipo;

namespace HD.Clientes.Consultas.Facturar_Equipo
{
    public class AD_Facturar_Equipo_Estado
    {
        private string CadenaConexion;
        public AD_Facturar_Equipo_Estado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_datos_view> informacion(string folio,int usuario)
        {
            try
            {
                var parametros = new
                { 
                    folio,
                    usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_JDF_Datos_Facturacion_View", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdl_datos_view data = new mdl_datos_view();
                data.datos_pedido = result.Read<mdl_datos_pedido>().FirstOrDefault();
                data.comentarios = result.Read<mdl_comentarios>().FirstOrDefault();
                data.sucursales = result.Read<mdl_sucursales_cliente>().ToList();
                data.financiamiento = result.Read<mdlPEdidoFinanciamiento>().ToList();
                factory.SQL.Close();
                if (data.datos_pedido == null) data.datos_pedido = new mdl_datos_pedido();
                if (data.comentarios == null) data.comentarios = new mdl_comentarios();
                return data;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
