using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.ObjetivosModelos;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace HD_Ventas.Consultas.ObjetivosModelos
{
    public class AD_Objetivos_Modelos_Guardar
    {
        private string CadenaConexion;
        public AD_Objetivos_Modelos_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Guarda los objetivos modificados. El detalle viaja al SP como JSON plano (una fila por celda)
        /// para desarmarse con OPENJSON y hacer upsert por idgeneral + ejercicio + idsucursal + periodo.
        /// El SP maneja su propia transaccion: si falla una celda no se guarda ninguna.
        /// Los errores definidos por el usuario en SQL (numero mayor o igual a 50000) son validaciones
        /// del SP con mensaje para el usuario final y se devuelven tal cual como BadRequest.
        /// </summary>
        public async Task Guardar(mdl_Objetivos_Modelos_Guardar mdl, int usuario)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    ejercicio = mdl.ejercicio,
                    idsucursal = mdl.idsucursal,
                    json = JsonConvert.SerializeObject(mdl.objetivos),
                    usuario = usuario
                };
                await factory.SQL.ExecuteAsync("Ventas.sp_Objetivos_Modelos_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
