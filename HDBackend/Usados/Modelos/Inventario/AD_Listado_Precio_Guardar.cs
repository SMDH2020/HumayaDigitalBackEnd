using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usados.Consultas.Inventario;

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
    }
}
