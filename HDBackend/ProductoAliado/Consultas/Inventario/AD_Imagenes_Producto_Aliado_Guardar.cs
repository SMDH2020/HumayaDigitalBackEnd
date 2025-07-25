using Dapper;
using HD.AccesoDatos;
using ProductoAliado.Modelos.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductoAliado.Consultas.Inventario
{
    public class AD_Imagenes_Producto_Aliado_Guardar
    {
        private string CadenaConexion;
        public AD_Imagenes_Producto_Aliado_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Imagenes_Producto_Aliado>> Guardar(mdl_Imagenes_Producto_Aliado mdl)
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
                    id_imagen = mdl.id_imagen,
                };
                IEnumerable<mdl_Imagenes_Producto_Aliado> result = await factory.SQL.QueryAsync<mdl_Imagenes_Producto_Aliado>("ProductoAliado.sp_Guardar_Imagen_Producto", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Imagenes_Producto_Aliado>> Buscar(int idinventario)
        {
            try
            {
                var parametros = new
                {
                    idinventario = idinventario,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Imagenes_Producto_Aliado> result = await factory.SQL.QueryAsync<mdl_Imagenes_Producto_Aliado>("ProductoAliado.sp_Listado_Precios_Obtener_Imagenes", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Imagenes_Producto_Aliado>> Eliminar(int id_imagen)
        {
            try
            {
                var parametros = new
                {
                    id_imagen = id_imagen,
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Imagenes_Producto_Aliado> result = await factory.SQL.QueryAsync<mdl_Imagenes_Producto_Aliado>("ProductoAliado.sp_Eliminar_Imagen_Producto", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
