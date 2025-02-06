using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Carga_Scorecard_porVendedor_Dash
    {
        private string CadenaConexion;
        public AD_Carga_Scorecard_porVendedor_Dash(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlCarga_Scorecard_porVendedor_Dash>> Scorecard(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Scorecard_porVendedor_Dash> result = await factory.SQL.QueryAsync<mdlCarga_Scorecard_porVendedor_Dash>("Ventas.Obtener_Scorecard_porUsuario", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                var tiposFaltantes = new List<string> { "Tractores", "Implementos", "Jardineros", "Autoguiado", "Drones", "Producto Aliado", "Tractores Usados", "Trilladoras Usadas" };

                // Asegurarse de que siempre haya un objeto por cada tipo de "tipo_cartera"
                var resultConTiposCompletos = tiposFaltantes.Select(tipo =>
                {
                    // Buscar el tipo en los datos obtenidos
                    var tipoExistente = result.FirstOrDefault(x => x.linea == tipo);

                    if (tipoExistente != null)
                    {
                        // Si el tipo ya existe, devolverlo tal como está
                        return tipoExistente;
                    }
                    else
                    {
                        // Si no existe el tipo, crear un objeto nuevo con todos los meses en 0
                        return new mdlCarga_Scorecard_porVendedor_Dash
                        {
                            linea = tipo,
                            objetivo = 0,
                            unidades_vendidas = 0,
                            importe = 0,
                            importe_proyectado = 0,
                            porcentaje = 0,
                            objetivo_acumulado = 0,
                            unidades_vendidas_acumulado = 0,
                            importe_acumulado = 0,
                            importe_proyectado_acumulado = 0,
                            porcentaje_acumulado = 0
                        };
                    }
                }).ToList();

                return resultConTiposCompletos;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
