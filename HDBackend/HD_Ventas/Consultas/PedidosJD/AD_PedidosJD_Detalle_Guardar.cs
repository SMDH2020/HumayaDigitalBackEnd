using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.PedidosJD;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace HD_Ventas.Consultas.PedidosJD
{
    public class AD_PedidosJD_Detalle_Guardar
    {
        private string CadenaConexion;
        public AD_PedidosJD_Detalle_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Guarda el detalle de un lote de pedidos a John Deere. Las tres listas viajan al SP como
        /// JSON plano (una fila por elemento) para desarmarse con OPENJSON. El SP hace todo en una
        /// sola transaccion: upsert de cabeceras y reemplazo completo de lineas y comentarios por pedido.
        /// Las cadenas vacias se mandan como null y las fechas en formato yyyy-MM-dd.
        /// Los errores definidos por el usuario en SQL (numero mayor o igual a 50000) son validaciones
        /// del SP con mensaje para el usuario final y se devuelven tal cual como BadRequest.
        /// </summary>
        public async Task<mdl_PedidosJD_Detalle_Guardar_Resultado> Guardar(mdl_PedidosJD_Detalle_Guardar mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                List<mdl_PedidosJD_Detalle_Cabecera> cabeceras = mdl.cabeceras ?? new List<mdl_PedidosJD_Detalle_Cabecera>();
                List<mdl_PedidosJD_Detalle_Linea> lineas = mdl.lineas ?? new List<mdl_PedidosJD_Detalle_Linea>();
                List<mdl_PedidosJD_Detalle_Comentario> comentarios = mdl.comentarios ?? new List<mdl_PedidosJD_Detalle_Comentario>();

                foreach (var item in cabeceras) VaciosANull(item);
                foreach (var item in lineas) VaciosANull(item);
                foreach (var item in comentarios) VaciosANull(item);

                var configuracionJson = new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd" };

                var parametros = new
                {
                    cuenta = mdl.cuenta,
                    fecha_consulta = mdl.fecha_consulta,
                    json_cabeceras = JsonConvert.SerializeObject(cabeceras, configuracionJson),
                    json_lineas = JsonConvert.SerializeObject(lineas, configuracionJson),
                    json_comentarios = JsonConvert.SerializeObject(comentarios, configuracionJson)
                };
                mdl_PedidosJD_Detalle_Guardar_Resultado result = await factory.SQL.QueryFirstAsync<mdl_PedidosJD_Detalle_Guardar_Resultado>("Ventas.sp_PedidosJD_Detalle_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
        private static void VaciosANull(object item)
        {
            foreach (var propiedad in item.GetType().GetProperties())
            {
                if (propiedad.PropertyType == typeof(string) && propiedad.CanWrite
                    && string.IsNullOrWhiteSpace((string?)propiedad.GetValue(item)))
                {
                    propiedad.SetValue(item, null);
                }
            }
        }
    }
}
