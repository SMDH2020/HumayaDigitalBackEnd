using Dapper;
using HD.AccesoDatos;
using ProductoAliado.Modelos.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProductoAliado.Consultas.Inventario
{
    public class AD_Listado_Precio_Producto_Aliado_Guardar
    {
        private string CadenaConexion;
        public AD_Listado_Precio_Producto_Aliado_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> ActualizarPrecio(mdl_Listado_Precio_Producto_Aliado mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idinventario = mdl.idinventario,
                    utilidad = mdl.utilidad,
                    margen = mdl.margen,
                    precio_lista = mdl.precio_lista,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("ProductoAliado.sp_Listado_Precio_Por_Unidad_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> ActualizarTodosPrecio(int idinventario, double utilidad, double margen, double precio_lista, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idinventario = idinventario,
                    utilidad = utilidad,
                    margen = margen,
                    precio_lista = precio_lista,
                    usuario = usuario
                };
                await factory.SQL.QueryAsync("ProductoAliado.sp_Listado_Precio_Por_Unidad_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> ActualizarListado(mdl_Inventario_Producto_Aliado mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idinventario = mdl.idinventario,
                    NE = mdl.NE,
                    Marca = mdl.Marca ,
                    modelo = mdl.modelo,
                    fecha_recepcion = mdl.fecha_recepcion,
                    ejercicio = mdl.ejercicio,
                    HP = mdl.HP,
                    sucursal = mdl.sucursal ,
                    serie = mdl.serie,
                    horas = mdl.horas ,
                    precio = mdl.precio ,
                    Costo = mdl.Costo ,
                    OT = mdl.OT ,
                    utilidad = mdl.utilidad,
                    margen = mdl.margen,
                    precio_lista = mdl.precio_lista,
                    estatus = mdl.estatus,
                    modelo_descripcion = mdl.modelo_descripcion,
                    usuario = mdl.usuario,

                };
                await factory.SQL.QueryAsync("ProductoAliado.sp_Listado_Precio_Actualizar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> GuardarPromocion(mdl_promocion_Producto_Aliado mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idpromocion = mdl.idpromocion,
                    idinventario = mdl.idinventario,
                    descripcion = mdl.descripcion,
                    vigencia = mdl.vigencia,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("ProductoAliado.sp_Listado_Precio_Promocion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Inventario_Producto_Aliado>> CambioEstado(int idinventario)
        {
            try
            {
                var parametros = new
                {
                    idinventario = idinventario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario_Producto_Aliado> result = await factory.SQL.QueryAsync<mdl_Inventario_Producto_Aliado>("ProductoAliado.sp_Cambio_Estatus", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
