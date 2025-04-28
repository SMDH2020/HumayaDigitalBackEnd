using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;
namespace HD_Ventas.Consultas
{
    public class AD_Agregar_Cotizacion
    {
        private string CadenaConexion;
        public AD_Agregar_Cotizacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Cotizaciones_AsesorDDL>> ListadoAsesores()
        {
            try
            {
                var parametros = new
                {
                    //usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Cotizaciones_AsesorDDL> result = await factory.SQL.QueryAsync<mdl_Cotizaciones_AsesorDDL>("Ventas.sp_Asesor_DropDownList", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl__Cotizaciones_ClientesSearch>> ListadoClientes()
        {
            try
            {
                var parametros = new
                {
                    //usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl__Cotizaciones_ClientesSearch> result = await factory.SQL.QueryAsync<mdl__Cotizaciones_ClientesSearch>("Ventas.sp_Cliente_Search", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Cotizaciones_SucursalesDDL>> ListadoSucursales()
        {
            try
            {
                var parametros = new
                {
                    //usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Cotizaciones_SucursalesDDL> result = await factory.SQL.QueryAsync<mdl_Cotizaciones_SucursalesDDL>("Ventas.sp_Sucursal_DropDownList", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> AgregarCotizacion(mdl_Agregar_Cotizacion mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcliente = mdl.idcliente,
                    idasesor = mdl.idasesor,
                    crm = mdl.crm,
                    asunto = mdl.asunto,
                    idsucursal = mdl.idsucursal,
                    tipo_pago = mdl.tipo_pago,
                    vigencia = mdl.vigencia,
                    usuario = mdl.usuario,
                    detalle = mdl.detalle
                };
                await factory.SQL.QueryAsync("Ventas.sp_Guardar_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
