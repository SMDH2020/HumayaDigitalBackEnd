using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.Cotizaciones;
using HD.Clientes.Modelos.CRM.Reportes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.CRM.Cotizaciones
{
    public class AD_Catalogo_ServicioCRM
    {
        private string CadenaConexion;
        public AD_Catalogo_ServicioCRM(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_Catalogo_ServicioCRM>> Listado()
        {
            try
            {
                var parametros = new
                {
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Catalogo_ServicioCRM> result = await factory.SQL.QueryAsync<mdl_Listado_Catalogo_ServicioCRM>("CRM.sp_Obtener_Catalogo_Servicio", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<int> Guardar(mdl_Catalogo_ServicioCRM mdl)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@id_servicio", mdl.IdServicio, DbType.Int32, ParameterDirection.InputOutput);
                parametros.Add("@id_linea_venta", mdl.IdLineaVenta, DbType.Int32);
                parametros.Add("@nombre_servicio", mdl.NombreServicio, DbType.String);
                parametros.Add("@descripcion", mdl.Descripcion, DbType.String);
                parametros.Add("@precio_lista", mdl.PrecioLista, DbType.Double);
                parametros.Add("@descuento", mdl.Descuento, DbType.Double);
                parametros.Add("@impuesto", mdl.Impuesto, DbType.Double);
                parametros.Add("@usuario", mdl.Usuario, DbType.Int32);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("CRM.sp_Catalogo_Servicio_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                return parametros.Get<int>("@id_servicio");
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> CambiarEstatus(int idServicio, bool estatus, int usuario)
        {
            try
            {
                var parametros = new
                {
                    id_servicio = idServicio,
                    estatus = estatus,
                    usuario = usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.ExecuteAsync("CRM.sp_Catalogo_Servicio_CambiarEstatus", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();

                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Catalogo_ServicioCRMID_View> ObtenerServicioID(int idServicio)
        {
            try
            {
                var parametros = new
                {
                    id_servicio = idServicio
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                using (var multi = await factory.SQL.QueryMultipleAsync("CRM.sp_Obtener_Servicio_ID", parametros, commandType: System.Data.CommandType.StoredProcedure))
                {
                    var lineasVenta = await multi.ReadAsync<mdl_Opciones_Lineas_Ventas>();
                    var servicio = await multi.ReadFirstAsync<mdl_Catalogo_Servicio_CRMID>();

                    var resultado = new mdl_Catalogo_ServicioCRMID_View
                    {
                        LineasVenta = lineasVenta,
                        Servicio = servicio
                    };

                    factory.SQL.Close();
                    return resultado;
                }
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
