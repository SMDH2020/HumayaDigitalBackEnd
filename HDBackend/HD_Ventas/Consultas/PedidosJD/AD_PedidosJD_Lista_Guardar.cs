using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.PedidosJD;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace HD_Ventas.Consultas.PedidosJD
{
    public class AD_PedidosJD_Lista_Guardar
    {
        private string CadenaConexion;
        public AD_PedidosJD_Lista_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Hace upsert de la lista de pedidos a John Deere. El detalle viaja al SP como JSON plano
        /// (una fila por pedido) para desarmarse con OPENJSON.
        /// Devuelve cuantos pedidos se recibieron, cuantos fueron nuevos y cuantos se actualizaron,
        /// mas el detalle de los pedidos pendientes de capturar su detalle (segundo result set del SP).
        /// Las cadenas vacias se mandan como null y las fechas en formato yyyy-MM-dd.
        /// </summary>
        public async Task<mdl_PedidosJD_Lista_Guardar_Resultado> Guardar(mdl_PedidosJD_Lista_Guardar mdl, int usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                List<mdl_PedidosJD_Pedido> pedidos = mdl.pedidos ?? new List<mdl_PedidosJD_Pedido>();
                foreach (mdl_PedidosJD_Pedido pedido in pedidos)
                    VaciosANull(pedido);

                var configuracionJson = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd" };

                var parametros = new
                {
                    cuenta = mdl.cuenta,
                    fecha_consulta = mdl.fecha_consulta,
                    json = JsonConvert.SerializeObject(pedidos, configuracionJson),
                    usuario = usuario
                };
                var resultSets = await factory.SQL.QueryMultipleAsync("Ventas.sp_PedidosJD_Lista_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_PedidosJD_Lista_Guardar_Resultado result = resultSets.ReadFirst<mdl_PedidosJD_Lista_Guardar_Resultado>();
                result.pendientes_detalle = resultSets.Read<mdl_PedidosJD_Pendiente>().ToList();
                factory.SQL.Close();
                return result;
            }
            catch (SqlException ex) when (ex.Number >= 50000)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.BadRequest, new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        // Convierte a null todas las propiedades string vacias o solo con espacios
        private static void VaciosANull(mdl_PedidosJD_Pedido pedido)
        {
            foreach (var propiedad in typeof(mdl_PedidosJD_Pedido).GetProperties())
            {
                if (propiedad.PropertyType == typeof(string) && propiedad.CanWrite
                    && string.IsNullOrWhiteSpace((string?)propiedad.GetValue(pedido)))
                {
                    propiedad.SetValue(pedido, null);
                }
            }
        }
    }
}
