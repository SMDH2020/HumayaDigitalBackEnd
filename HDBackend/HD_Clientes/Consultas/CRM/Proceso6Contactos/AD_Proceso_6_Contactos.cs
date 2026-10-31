using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Proceso6Contactos;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.CRM.Proceso6Contactos
{
    public class AD_Proceso_6_Contactos
    {
        private readonly string CadenaConexion;

        public AD_Proceso_6_Contactos(string cadenaConexion)
        {
            CadenaConexion = cadenaConexion;
        }

        public async Task<IEnumerable<mdl_Proceso_6Contactos_Listado>> Listado(
            string fecha_inicio, string fecha_fin, string adr, string sucursal, int usuario)
        {
            try
            {
                var parametros = new
                {
                    fecha_inicio,
                    fecha_fin,
                    adr,
                    sucursal,
                    usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Proceso_6Contactos_Listado> result =
                    await factory.SQL.QueryAsync<mdl_Proceso_6Contactos_Listado>(
                        "Ventas.sp_Proceso_6Contactos_Listado",
                        parametros,
                        commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Timeline_Equipo_Entregado>> ObtenerTimeline(int id_CEE)
        {
            try
            {
                var parametros = new { id_CEE };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Timeline_Equipo_Entregado> result =
                    await factory.SQL.QueryAsync<mdl_Timeline_Equipo_Entregado>(
                        "Ventas.sp_Obtener_Timeline_Equipo_Entregado",
                        parametros,
                        commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> IndicarRealizado(mdl_Indicar_Realizado mdl)
        {
            try
            {
                var parametros = new
                {
                    id_c6c = mdl.id_c6c,
                    comentario = mdl.comentario,
                    usuario = mdl.usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync(
                    "Ventas.sp_Proceso_6contactos_Indicar_Realizado",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Proceso_6Contactos_Reporte>> Reporte(
            string fecha_inicio, string fecha_fin, string adr, string sucursal,
            string fase, int responsable, int usuario)
        {
            try
            {
                var parametros = new
                {
                    fecha_inicio,
                    fecha_fin,
                    adr,
                    sucursal,
                    fase,
                    responsable,
                    usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Proceso_6Contactos_Reporte> result =
                    await factory.SQL.QueryAsync<mdl_Proceso_6Contactos_Reporte>(
                        "Ventas.sp_Proceso_6Contactos_Reporte",
                        parametros,
                        commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Filtro_Responsable_C6C>> FiltroResponsables(
            string fecha_inicio, string fecha_fin)
        {
            try
            {
                var parametros = new
                {
                    fecha_inicio,
                    fecha_fin
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Filtro_Responsable_C6C> result =
                    await factory.SQL.QueryAsync<mdl_Filtro_Responsable_C6C>(
                        "Ventas.sp_Get_Filtro_Responsables_C6C",
                        parametros,
                        commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Notifica_Proceso_6_Contactos>> Proceso6ContactosNotificar()
        {
            try
            {
                var parametros = new
                {
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Notifica_Proceso_6_Contactos> result =
                    await factory.SQL.QueryAsync<mdl_Notifica_Proceso_6_Contactos>(
                        "Ventas.sp_notificar_proceso_6_contactos",
                        parametros,
                        commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }
    }
}
