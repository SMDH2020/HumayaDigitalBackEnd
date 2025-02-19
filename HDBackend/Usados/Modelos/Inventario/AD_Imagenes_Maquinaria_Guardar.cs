using Dapper;
using HD.AccesoDatos;
using Usados.Consultas.Inventario;

namespace Usados.Modelos.Inventario
{
    public class AD_Imagenes_Maquinaria_Guardar
    {
        private string CadenaConexion;
        public AD_Imagenes_Maquinaria_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdl_Imagenes_Maquinaria mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    idinventario = mdl.idinventario,
                    documento = mdl.documento,
                    extension = mdl.extension,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Usados.sp_Guardar_Imagen_Maquinaria", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
