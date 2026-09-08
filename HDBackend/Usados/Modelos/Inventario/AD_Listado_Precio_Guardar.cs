using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usados.Consultas.Inventario;
using Usados.Consultas.Usados;

namespace Usados.Modelos.Inventario
{
    public class AD_Listado_Precio_Guardar
    {
        private string CadenaConexion;
        public AD_Listado_Precio_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> ActualizarPrecio(mdl_Listado_Precio mdl)
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
                await factory.SQL.QueryAsync("Usados.sp_Listado_Precio_Por_Unidad_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
                await factory.SQL.QueryAsync("Usados.sp_Listado_Precio_Por_Unidad_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> ActualizarListado(mdl_Inventario mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idinventario = mdl.idinventario,
                    NE = mdl.NE,
                    Marca = mdl.Marca,
                    modelo = mdl.modelo,
                    fecha_recepcion = mdl.fecha_recepcion,
                    ejercicio = mdl.ejercicio,
                    HP = mdl.HP,
                    sucursal = mdl.sucursal,
                    serie = mdl.serie,
                    horas = mdl.horas,
                    precio = mdl.precio,
                    Costo = mdl.Costo,
                    OT = mdl.OT,
                    utilidad = mdl.utilidad,
                    margen = mdl.margen,
                    precio_lista = mdl.precio_lista,
                    estatus = mdl.estatus,
                    modelo_descripcion = mdl.modelo_descripcion,
                    usuario = mdl.usuario,

                };
                await factory.SQL.QueryAsync("Usados.sp_Listado_Precio_Actualizar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> GuardarPromocion(mdl_promocion mdl)
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
                await factory.SQL.QueryAsync("Usados.sp_Listado_Precio_Promocion_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Inventario>> CambioEstado(int idinventario)
        {
            try
            {
                var parametros = new
                {
                    idinventario = idinventario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario> result = await factory.SQL.QueryAsync<mdl_Inventario>("Usados.sp_Cambio_Estatus", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
