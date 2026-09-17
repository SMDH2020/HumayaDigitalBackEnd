using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM;
using HD.Clientes.Modelos.CRM.Visitas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.CRM.Visitas
{
    public class AD_Visitas_CRM
    {
        private string CadenaConexion;
        public AD_Visitas_CRM(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Visitas_Programada_View> ListadoVisitasProgramadas(int ejercicio, int periodo, string fechainicio, string fechafin, int vendedor, string adr, string sucursal)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    vendedor = vendedor,
                    fechainicio = fechainicio,
                    fechafin = fechafin,
                    adr = adr,
                    sucursal = sucursal
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("CRM.sp_Obtener_Visitas_Programadas_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdl_Visitas_Programada_View mdl = new mdl_Visitas_Programada_View();
                mdl.listado_visitas = result.Read<mdl_Listado_Visitas_Programadas>().ToList();
                mdl.header_info = result.Read<mdl_Header_Info_Visitas_Programadas>().FirstOrDefault();
                mdl.opciones_tipo_visita = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_lineas = result.Read<mdl_Opciones_Lineas_CRM>().ToList();
                mdl.permisos = result.Read<mdl_Permisos_CRM>().FirstOrDefault();


                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Listado_Visitas_Programadas> ObtenerVisitaID(int idvisita)
        {
            try
            {
                var parametros = new
                {
                    idvisita = idvisita
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_Listado_Visitas_Programadas result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Listado_Visitas_Programadas>("CRM.sp_Obtener_Visita_Programada_ID", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<long> ProgramarVisita(mdl_Programar_Visita_CRM mdl)
        {
            try
            {
                var parametros = new
                {
                    idvisita = mdl.idvisita,
                    idcliente = mdl.idcliente,
                    idvendedor = mdl.usuario,
                    tipo_visita = mdl.tipo_visita,
                    fecha_visita = mdl.fecha_visita,
                    notas = mdl.notas,
                    usuario = mdl.usuario,
                    estatus = mdl.estatus,
                    linea = mdl.linea
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                long result = await factory.SQL.QueryFirstOrDefaultAsync<long>("CRM.sp_Programar_Visita", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Visita_TimeLine_CRM>> ObtenerTimeLine(long idvisita)
        {
            try
            {
                var parametros = new
                {
                    idvisita = idvisita
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Visita_TimeLine_CRM> result = await factory.SQL.QueryAsync<mdl_Visita_TimeLine_CRM>("CRM.sp_Obtener_Visita_TimeLine", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task GuardarEstatusVisita(mdl_Guarda_Estatus_Visita_CRM mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    idvisita = mdl.idvisita,
                    estatus = mdl.estatus,
                    comentario = mdl.comentario,
                    createuser = mdl.createuser
                };

                await factory.SQL.ExecuteAsync("CRM.sp_Guardar_Estatus_Visita", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
            }
            catch (System.Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task EliminaVisita(int id_visita)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    id_visita = id_visita
                };

                await factory.SQL.ExecuteAsync("CRM.sp_Elimina_Visita", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
            }
            catch (System.Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
