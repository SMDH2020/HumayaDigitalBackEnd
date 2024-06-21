using Dapper;
using HD.AccesoDatos;
using HD_Buro.Modelos;

namespace HD_Buro.Consultas
{
    public class AD_Carga_Detalle_Buro_Vencido
    {
        private string CadenaConexion;
        public AD_Carga_Detalle_Buro_Vencido(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlCarga_Detalle_Buro>> detalle_vencido(int ejercicio, int periodo, string idcliente)
        {
            try
            {
                var parametros = new
                {
                    Ejercicio = ejercicio,
                    Periodo = periodo,
                    idCliente = idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Detalle_Buro> result = await factory.SQL.QueryAsync<mdlCarga_Detalle_Buro>("Cartera_clientes.dbo.sp_Cargar_Detalle_Buro_Vencido", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
