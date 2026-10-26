using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.Inventario;

namespace HD_Ventas.Consultas.Inventario
{
    public class AD_Inventario_Modelos_Guardar
    {
        private string CadenaConexion;
        public AD_Inventario_Modelos_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task Guardar(mdl_Inventario_Modelos_Listado mdl, int usuario)
        {
            try
            {
                var parametros = new
                {
                    idGeneral = mdl.idGeneral,
                    linea = mdl.linea,
                    modelo = mdl.modelo,
                    descripcion = mdl.descripcion,
                    departamento = mdl.departamento,
                    categoria = mdl.categoria,
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("Ventas.sp_Inventario_Modelos_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
