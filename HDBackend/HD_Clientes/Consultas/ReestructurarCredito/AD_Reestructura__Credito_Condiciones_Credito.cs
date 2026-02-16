using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.ReestructurarCredito
{
    public class AD_Reestructura__Credito_Condiciones_Credito
    {
        private string CadenaConexion;
        public AD_Reestructura__Credito_Condiciones_Credito(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdlCondiciones_Venta_View> Obtener(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Reestructura_Credito_Condiciones_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlCondiciones_Venta_View view = new mdlCondiciones_Venta_View();
                view.condiciones = result.Read<mdlPedido_Condiciones_Venta>().FirstOrDefault();
                view.interes = result.Read<mdlInteres_Credito>().FirstOrDefault();
                factory.SQL.Close();
                //if (view.mdlSolicitud == null) view.mdlSolicitud = new mdlSolicitudCredito_Enviar();
                if (view.condiciones == null) view.condiciones = new mdlPedido_Condiciones_Venta();
                if (view.interes == null) view.interes = new mdlInteres_Credito();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
