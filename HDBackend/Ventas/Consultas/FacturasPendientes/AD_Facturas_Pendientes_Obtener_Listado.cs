using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.Facturar_Equipo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Consultas.FacturasPendientes
{
    public class AD_Facturas_Pendientes_Obtener_Listado
    {
        private string CadenaConexion;
        public AD_Facturas_Pendientes_Obtener_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlListFacCerrada>> Listado(string usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlListFacCerrada> result = await factory.SQL.QueryAsync<mdlListFacCerrada>("Credito.sp_Solicitud_Credito_Tablas_Facturacion_Pendiente", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
