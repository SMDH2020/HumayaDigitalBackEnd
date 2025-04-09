using Dapper;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usados.Consultas.Usados;
 
namespace Usados.Modelos.Usados
{
    public class AD_Inventario_Listado
    {
        private string CadenaConexion;
        public AD_Inventario_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Inventario>> Listado(string Modelo, int ejercicio, string HP, string Sucursal, string Promocion, string Estatus)
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
                IEnumerable<mdl_Inventario> result = await factory.SQL.QueryAsync<mdl_Inventario>("Usados.sp_Inventario_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
 
        public async Task<IEnumerable<mdl_Inventario>> ListadoFiltro()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario> result = await factory.SQL.QueryAsync<mdl_Inventario>("Usados.sp_Inventario_Listado_Filtros", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Inventario>> ListadoActual()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario> result = await factory.SQL.QueryAsync<mdl_Inventario>("Usados.sp_Inventario_Listado_Precio_Actual", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Inventario>> ListadoFiltroMovil()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Inventario> result = await factory.SQL.QueryAsync<mdl_Inventario>("Usados.sp_Inventario_Listado_Movil", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                IEnumerable<mdl_Inventario> result2 = result
                    .OrderBy(x => x.modelo_descripcion.Contains("TR-")
                                     || x.modelo_descripcion.Contains("TR ")
                                            && !x.modelo_descripcion.Contains("TRILLADORA")
                                            && !x.modelo_descripcion.Contains("TRASN")
                                            && !x.modelo_descripcion.Contains("TRANS")
                                     || x.modelo_descripcion.Contains("TRACTOR")
                    ? 0 :
                    x.modelo_descripcion.Contains("TRILLADORA") ? 1 :
                    x.modelo_descripcion.Contains("CABEZAL") ? 2 :
                    3)  // Los que no contienen esas palabras
                .ThenBy(x => x.estatus == "L" ? 0 : x.estatus == "A" ? 1 : 2) // Ordenar por estatus
                .ThenBy(det => det.sucursal)
                .ThenBy(det => det.HP)
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