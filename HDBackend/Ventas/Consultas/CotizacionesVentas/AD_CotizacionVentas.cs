using Dapper;
using HD.AccesoDatos;
using Ventas.Modelos.CotizacionesVentas;

namespace Ventas.Consultas.CotizacionesVentas
{
    public class AD_CotizacionVentas
    {
        private string CadenaConexion;
        public AD_CotizacionVentas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlCotizacionVentaSearch> ObtenerByFolio(string usuario, string folio)
        {
            try
            {
                var parametros = new
                {
                    folio,
                    usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("ventas.sp_cotizaciones_ventas_Folio", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlCotizacionVentaSearch mdl = new mdlCotizacionVentaSearch();
                mdl.cotizacion = result.Read<mdlCotizacionVentas>().FirstOrDefault();
                mdl.detalle = result.Read<mdlCotizacionVentaDetalle>().ToList();
                mdl.rol = result.Read<mdlCotizacionVenta_rol>().FirstOrDefault();
                mdl.clientes = result.Read<mdlCotizacionVentaDropdownlist>().ToList();
                mdl.asesorventas = result.Read<mdlCotizacionVentaDropdownlist>().ToList();
                mdl.esquemas = result.Read<mdlCotizacionVentaDropdownlist>().ToList();
                mdl.modelos = result.Read<mdlCotizacionVentasModelos>().ToList();

                if (mdl.cotizacion is null) mdl.cotizacion = new mdlCotizacionVentas();
                factory.SQL.Close();
                return mdl;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> AgregarCotizacion(mdl_Agregar_Cotizacion_Nuevo mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    idcliente = mdl.idcliente,
                    idasesor = mdl.idasesor,
                    razon_social = mdl.razon_social,
                    vigencia = mdl.vigencia,
                    idesquema = mdl.idesquema,
                    moneda = mdl.moneda,
                    mostrar_precio_lista = mdl.mostrar_precio_lista,
                    usuario = mdl.usuario,
                    detalle = mdl.detalle
                };
                await factory.SQL.QueryAsync("Ventas.sp_Guardar_Cotizacion_Nuevo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Cotizaciones_Nuevo>> Listado(int usuario, string comparacion, string periodoinicio, string periodofin, string adr, string sucursal, int asesor, int cliente, int esquema, string fase)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario,
                    comparacion = comparacion,
                    periodoinicio = periodoinicio,
                    periodofin = periodofin,
                    adr = adr,
                    sucursal = sucursal,
                    asesor = asesor,
                    cliente = cliente,
                    esquema = esquema,
                    fase = fase
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Cotizaciones_Nuevo> result = await factory.SQL.QueryAsync<mdl_Listado_Cotizaciones_Nuevo>("Ventas.sp_Obtener_Listado_Cotizaciones_Nuevo", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Listado_Cotizaciones_Nuevo>> Eliminar_Cotizacion(string folio, int usuario)
        {
            try
            {
                var parametros = new
                {
                    folio = folio,
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Cotizaciones_Nuevo> result = await factory.SQL.QueryAsync<mdl_Listado_Cotizaciones_Nuevo>("Ventas.sp_Eliminar_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Filtros_DropDownList>> DDLAsesores(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Filtros_DropDownList> result = await factory.SQL.QueryAsync<mdl_Filtros_DropDownList>("Ventas.get_filtro_cotizaciones_asesores", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Filtros_DropDownList>> DDLClientes()
        {
            try
            {
                var parametros = new
                {
                    //usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Filtros_DropDownList> result = await factory.SQL.QueryAsync<mdl_Filtros_DropDownList>("Ventas.get_filtro_cotizaciones_clientes", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Filtros_DropDownList>> DDLEsquemas()
        {
            try
            {
                var parametros = new
                {
                    //usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Filtros_DropDownList> result = await factory.SQL.QueryAsync<mdl_Filtros_DropDownList>("Ventas.get_filtro_cotizaciones_esquemas", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Permisos_Cotizaciones> GetPermisos(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                mdl_Permisos_Cotizaciones result = await factory.SQL.QueryFirstOrDefaultAsync<mdl_Permisos_Cotizaciones>("Ventas.sp_GetPermisos_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
