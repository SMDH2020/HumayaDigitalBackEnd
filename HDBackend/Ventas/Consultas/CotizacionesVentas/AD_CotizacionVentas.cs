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
        public async Task<mdlCotizacionVentaSearch> ObtenerByFolio(string usuario,string folio)
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
                mdl.cotizacion=result.Read<mdlCotizacionVentas>().FirstOrDefault();
                mdl.detalle=result.Read<mdlCotizacionVentaDetalle>().ToList();  
                mdl.rol=result.Read<mdlCotizacionVenta_rol>().FirstOrDefault();
                mdl.clientes = result.Read<mdlCotizacionVentaDropdownlist>().ToList();
                mdl.asesorventas = result.Read<mdlCotizacionVentaDropdownlist>().ToList();
                mdl.esquemas = result.Read<mdlCotizacionVentaDropdownlist>().ToList();


                if (mdl.cotizacion is null) mdl.cotizacion = new mdlCotizacionVentas(); 
                factory.SQL.Close();
                return mdl;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
