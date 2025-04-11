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
    public class AD_Inventario_Listado
    {
        private string CadenaConexion;
        public AD_Inventario_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Inventario_Producto_Aliado>> Listado(string Modelo, int ejercicio, string HP, string Sucursal, string Promocion, string Estatus)
        {
            try
            {
                var parametros = new
                {
                    Modelo = Modelo,
                    ejercicio = ejercicio,
                    HP = HP,
                    Sucursal = Sucursal,
                    Promocion = Promocion,
                    Estatus = Estatus
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario_Producto_Aliado> result = await factory.SQL.QueryAsync<mdl_Inventario_Producto_Aliado>("ProductoAliado.sp_Inventario_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Inventario_Producto_Aliado>> ListadoFiltro()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario_Producto_Aliado> result = await factory.SQL.QueryAsync<mdl_Inventario_Producto_Aliado>("ProductoAliado.sp_Inventario_Listado_Filtros", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }



        public async Task<IEnumerable<mdl_Inventario_Producto_Aliado>> ListadoPrecioActual()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario_Producto_Aliado> result = await factory.SQL.QueryAsync<mdl_Inventario_Producto_Aliado>("ProductoAliado.sp_Listado_Precio_Actual", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Inventario_Producto_Aliado>> ListadoPrecioActualMovil()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario_Producto_Aliado> result = await factory.SQL.QueryAsync<mdl_Inventario_Producto_Aliado>("ProductoAliado.sp_Listado_Precio_Actual_Movil", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                IEnumerable<mdl_Inventario_Producto_Aliado> result2 = result
                    .OrderBy(det =>
                    det.modelo_descripcion.Contains("ASPERSORA") ? 0 :
                    det.modelo_descripcion.Contains("REMOLQUE") ? 1 :
                    det.modelo_descripcion.Contains("MOLINO") ? 2 :
                    3)  // Los que no contienen esas palabras
                .ThenBy(det => det.estatus == "L" ? 0 : det.estatus == "A" ? 1 : 2) // Ordenar por estatus
                .ThenBy(det => det.sucursal) // Ordenar por sucursal
                .ToList();
                return result2;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
