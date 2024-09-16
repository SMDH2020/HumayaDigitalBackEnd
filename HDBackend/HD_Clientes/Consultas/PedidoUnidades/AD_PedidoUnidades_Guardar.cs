using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.PedidoUnidades
{
    public class AD_PedidoUnidades_Guardar
    {
        private string CadenaConexion;
        public AD_PedidoUnidades_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlPedido_Unidades mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    registro = mdl.registro,
                    nuevo = mdl.nuevo,
                    modelo = mdl.modelo,
                    descripcion=mdl.descripcion,
                    serie = mdl.serie,
                    cantidad = mdl.cantidad,
                    precio = mdl.precio,
                    descuento = mdl.descuento,
                    usuario=mdl.usuario,
                };
                await factory.SQL.QueryAsync("Credito.sp_Pedido_Unidades_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
