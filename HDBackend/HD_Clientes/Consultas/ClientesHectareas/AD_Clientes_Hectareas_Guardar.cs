using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;

namespace HD.Clientes.Consultas.Clientes_Hectareas
{
    public class AD_Clientes_Hectareas_Guardar
    {
        private string CadenaConexion;
        public AD_Clientes_Hectareas_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdlClientes_Hectareas mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    registro = mdl.registro,
                    hectareas_propias = mdl.hectareas_propias,
                    hectareas_rentadas = mdl.hectareas_rentadas,
                    hectareas_ejidal = mdl.hectareas_ejidal,
                    hectareas_sociedad = mdl.hectareas_sociedad,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Credito.clientes_hectareas_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
