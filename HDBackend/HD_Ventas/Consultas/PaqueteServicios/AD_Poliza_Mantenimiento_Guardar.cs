using Dapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using HD.AccesoDatos;
using HD_Ventas.Modelos.PaqueteServicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas.PaqueteServicios
{
    public class AD_Poliza_Mantenimiento_Guardar
    {
        private string CadenaConexion;
        public AD_Poliza_Mantenimiento_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<bool> Guardar(mdl_Poliza_Mantenimiento_Guardar mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idpoliza  = mdl.idpoliza,
                    idsucursal = mdl.idsucursal,
                    cliente = mdl.idcliente,
                    serie = mdl.serie,
                    periodo = mdl.periodo,
                    num_factura = mdl.num_factura,
                    tipo = mdl.tipo,
                    mano_obra = mdl.mano_obra,
                    refacciones = mdl.refacciones,
                    km = mdl.km,
                    facturacion = mdl.facturacion ,
                    orden_trabajo = mdl.orden_trabajo ,
                    ejercicio = mdl.ejercicio ,
                    mes = mdl.mes ,
                    vendedor = mdl.idvendedor ,
                    usuario = mdl.usuario 
                };
                await factory.SQL.ExecuteAsync("Ventas.sp_Poliza_Mantenimiento_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
