using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.ReestructurarCredito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.ReestructurarCredito
{
    public class AD_Reestructurar_Credito_Informacion_Solicitud
    {
        private string CadenaConexion;
        public AD_Reestructurar_Credito_Informacion_Solicitud(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Reestructurar_Credito_Informacion_Solicitud> Obtener(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Reestructura_Credito_Informacion_Solicitud", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Reestructurar_Credito_Informacion_Solicitud view = new mdl_Reestructurar_Credito_Informacion_Solicitud();
                view.condiciones = result.Read<mdlPedido_Condiciones_Venta>().FirstOrDefault();
                view.detalles_financiamiento = result.Read<mdlPedido_Detalle_Financiamiento>().ToList();
                factory.SQL.Close();
                //if (view.mdlSolicitud == null) view.mdlSolicitud = new mdlSolicitudCredito_Enviar();
                if (view.condiciones == null) view.condiciones = new mdlPedido_Condiciones_Venta();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
