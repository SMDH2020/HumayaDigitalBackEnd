using Dapper;
using HD.AccesoDatos;
using HD_Mensajeria.Modelos;

namespace HD_Mensajeria.Consultas
{
    public class AD_Leads_Agente
    {
        private string CadenaConexion;
        public AD_Leads_Agente(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Leads_Agente_View> obtenerLeads()
        {
            try
            {
                var parametros = new
                {
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("HD_Mensajeria.dbo.sp_Obtener_Leads_Agente_HD", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Leads_Agente_View mdl = new mdl_Leads_Agente_View();
                mdl.Leads = result.Read<mdl_Leads_Agente>().ToList();
                mdl.Empleados = result.Read<mdl_Empleados_Leads>().ToList();
                mdl.Sucursales = result.Read<mdl_Sucursales_Leads>().ToList();
                mdl.Areas = result.Read<mdl_Areas_Leads>().ToList();

                factory.SQL.Close();
                return mdl;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> AgregarLead(mdl_Guardar_Leads_Agente mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    id_usuario = mdl.id_usuario,
                    area = mdl.area,
                    sucursal = mdl.sucursal,
                    tipo_usuario = mdl.tipo_usuario,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("HD_Mensajeria.dbo.sp_Guardar_Leads_Agente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> ActualizarLead(mdl_Actualiza_Lead mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    id_lead = mdl.id_lead,
                    tipo_usuario = mdl.tipo_usuario,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("HD_Mensajeria.dbo.sp_Actualizar_Leads_Agente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> EliminaLead(int id_lead)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    id_lead = id_lead,
                };
                await factory.SQL.QueryAsync("HD_Mensajeria.dbo.sp_Elimina_Leads_Agente", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> CambiaEstatusLead(int id_lead, int usuario)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    id_lead = id_lead,
                    usuario = usuario
                };
                await factory.SQL.QueryAsync("HD_Mensajeria.dbo.sp_Change_Estatus_Lead_Agente", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
