using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas
{
    public class AD_Guardar_Facturacion_Cotizacion
    {
        private string CadenaConexion;
        public AD_Guardar_Facturacion_Cotizacion(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<string> ModificarCotizacion(mdl_Facturacion_Guardar mdl)
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio_cotizacion = mdl.folio_cotizacion,
                    fecha_entrega = mdl.fecha_entrega,
                    entregado = mdl.entregado,
                    folio_solicitud = mdl.folio_solicitud,
                    contacto_servicio = mdl.contacto_servicio,
                    contacto_refacciones = mdl.contacto_refacciones,
                    usuario = mdl.usuario,
                    fase = mdl.fase,
                    idcliente = mdl.idcliente
                };
                string? nombre_cliente = await factory.SQL.QueryFirstOrDefaultAsync<string>("Ventas.sp_Guardar_Factura_Cotizacion", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return nombre_cliente;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
