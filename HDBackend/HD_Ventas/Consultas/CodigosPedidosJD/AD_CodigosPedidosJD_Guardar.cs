using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.CodigosPedidosJD;
using System.Data.SqlClient;

namespace HD_Ventas.Consultas.CodigosPedidosJD
{
    public class AD_CodigosPedidosJD_Guardar
    {
        private string CadenaConexion;
        public AD_CodigosPedidosJD_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Los errores definidos por el usuario en SQL (numero mayor o igual a 50000) son validaciones
        /// del SP con mensaje para el usuario final y se devuelven tal cual como BadRequest.
        /// </summary>
        public async Task Guardar(mdl_CodigosPedidosJD_Guardar mdl, int usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    codigo = mdl.codigo,
                    clave = mdl.clave,
                    descripcion = mdl.descripcion,
                    tipo = mdl.tipo,
                    usuario = usuario
                };
                await factory.SQL.ExecuteAsync("Ventas.sp_Codigos_Pedidosjd_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
