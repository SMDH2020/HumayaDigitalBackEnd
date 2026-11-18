using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.AnalisisInventarioModelos;
using System.Data.SqlClient;

namespace HD_Ventas.Consultas.AnalisisInventarioModelos
{
    public class AD_AnalisisInventarioModelos_RelacionModelos_Guardar
    {
        private string CadenaConexion;
        public AD_AnalisisInventarioModelos_RelacionModelos_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Los errores definidos por el usuario en SQL (numero mayor o igual a 50000) son validaciones
        /// del SP con mensaje para el usuario final y se devuelven tal cual como BadRequest.
        /// </summary>
        public async Task Guardar(mdl_AnalisisInventarioModelos_RelacionModelos_Guardar mdl, int usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    modelo = mdl.modelo,
                    idgeneral = mdl.idgeneral,
                    usuario = usuario
                };
                await factory.SQL.ExecuteAsync("Ventas.sp_Relacion_Modelos_Generales_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
