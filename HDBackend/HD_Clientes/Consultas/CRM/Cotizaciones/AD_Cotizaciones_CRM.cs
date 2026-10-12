using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Consultas.SolicitudCreditoDocumento;
using HD.Clientes.Modelos.CRM;
using HD.Clientes.Modelos.CRM.Cotizaciones;
using HD.Clientes.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.CRM.Cotizaciones
{
    public class AD_Cotizaciones_CRM
    {
        private string CadenaConexion;
        public AD_Cotizaciones_CRM(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<string> Guardar(mdl_Cotizaciones_CRM_Guarad mdl)
        {
            try
            {

                var parametros = new DynamicParameters();
                parametros.Add("@folio", mdl.folio, DbType.StringFixedLength, ParameterDirection.InputOutput, size: 13);
                parametros.Add("@datos", mdl.datos, DbType.String);
                parametros.Add("@usuario", mdl.usuario, DbType.Int32);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("CRM.sp_Cotizaciones_Servicio_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                return parametros.Get<string>("@folio");
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> Eliminar(string folio, int usuario)
        {
            try
            {
                var parametros = new
                {
                    folio = folio,
                    usuario = usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("CRM.sp_Cotizaciones_Servicio_Eliminar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Listado_Cotizaciones_CRM_View> Listado(string fechainicio, string fechafin, string adr, string sucursal, int usuario)
        {
            try
            {
                var parametros = new
                {
                    fechainicio = fechainicio,
                    fechafin = fechafin,
                    adr = adr,
                    sucursal = sucursal,
                    usuario = usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("CRM.sp_Obtener_Listado_Cotizaciones_ServicioCRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Listado_Cotizaciones_CRM_View mdl = new mdl_Listado_Cotizaciones_CRM_View();
                mdl.cotizaciones = result.Read<mdl_Listado_Cotizaciones_CRM>().ToList();
                mdl.permisos = result.Read<mdl_Permisos_CRM>().FirstOrDefault();

                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Cotizaciones_CRM_Folio_View> ObtenerPorFolio(string folio, int usuario)
        {
            try
            {
                var parametros = new
                {
                    folio = folio,
                    usuario = usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("CRM.sp_Obtener_Cotizaciones_ServicioCRM_folio", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Cotizaciones_CRM_Folio_View mdl = new mdl_Cotizaciones_CRM_Folio_View();
                mdl.Clientes = result.Read<mdl_Opciones_Clientes_Cotizacion_CRM>().ToList();
                mdl.Asesores = result.Read<mdl_Opciones_Asesores_Cotizaciones_CRM>().ToList();
                mdl.Origenes = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.TiposPago = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.LineaCotizacion = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.Cotizacion = result.Read<mdl_Cotizaciones_CRM_Folio>().FirstOrDefault();
                mdl.Detalle = result.Read<mdl_Cotizaciones_CRM_Folio_Detalle>().ToList();
                mdl.caracteristicas = result.Read<mdl_Cotizaciones_CRM_Folio_Caracteristicas>().ToList();
                mdl.permisos = result.Read<mdl_Permisos_CRM>().FirstOrDefault();
                mdl.responsables_contacto = result.Read<mdl_Responsables_Contacto_Cotizaciones_CRM>()?.ToList();


                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Responsables_Contacto_Cotizaciones_CRM>> GetOpcionesContacto(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Responsables_Contacto_Cotizaciones_CRM> result = await factory.SQL.QueryAsync<mdl_Responsables_Contacto_Cotizaciones_CRM>("CRM.Get_Responsables_Contacto_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }


        public async Task<IEnumerable<mdl_Opciones_Correos_Cliente_Cotizaciones_CRM>> GetOpcionesCorreo(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Opciones_Correos_Cliente_Cotizaciones_CRM> result = await factory.SQL.QueryAsync<mdl_Opciones_Correos_Cliente_Cotizaciones_CRM>("CRM.Listado_Correos_Cliente_Cotizaciones_Servicio", parametros, commandType: System.Data.CommandType.StoredProcedure);
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