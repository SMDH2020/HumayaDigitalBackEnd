using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.InventarioModelo;
using System.Data.SqlClient;

namespace HD_Ventas.Consultas.InventarioModelo
{
    public class AD_InventarioModelo_Excluir
    {
        private string CadenaConexion;
        public AD_InventarioModelo_Excluir(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Los errores definidos por el usuario en SQL (numero mayor o igual a 50000) son validaciones
        /// del SP con mensaje para el usuario final y se devuelven tal cual como BadRequest.
        /// </summary>
        public async Task Excluir(mdl_InventarioModelo_Excluir mdl, int usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    modelo = mdl.modelo,
                    accion = mdl.accion,
                    usuario = usuario
                };
                await factory.SQL.ExecuteAsync("Ventas.sp_Inventario_Modelos_Excluir", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
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
    }
}
