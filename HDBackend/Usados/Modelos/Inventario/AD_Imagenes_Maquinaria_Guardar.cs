using Dapper;
using HD.AccesoDatos;
using Usados.Consultas.Inventario;
using Usados.Consultas.Usados;

namespace Usados.Modelos.Inventario
{
    public class AD_Imagenes_Maquinaria_Guardar
    {
        private string CadenaConexion;
        public AD_Imagenes_Maquinaria_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Imagenes_Maquinaria>> Guardar(mdl_Imagenes_Maquinaria mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {

                var parametros = new
                {
                    idinventario = mdl.idinventario,
                    documento = mdl.documento,
                    extension = mdl.extension,
                    usuario = mdl.usuario,
                    id_imagen=mdl.id_imagen
                };
                IEnumerable<mdl_Imagenes_Maquinaria> result = await factory.SQL.QueryAsync<mdl_Imagenes_Maquinaria>("Usados.sp_Guardar_Imagen_Maquinaria", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Imagenes_Maquinaria>> Buscar(int idinventario)
        {
            try
            {
                var parametros = new
                {
                    idinventario = idinventario,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Imagenes_Maquinaria> result = await factory.SQL.QueryAsync<mdl_Imagenes_Maquinaria>("Usados.sp_Obtener_Imagen_Maquinaria", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Imagenes_Maquinaria>> Eliminar(int id_imagen)
        {
            try
            {
                var parametros = new
                {
                    id_imagen = id_imagen,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Imagenes_Maquinaria> result = await factory.SQL.QueryAsync<mdl_Imagenes_Maquinaria>("Usados.sp_Eliminar_Imagen_Maquinaria", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
