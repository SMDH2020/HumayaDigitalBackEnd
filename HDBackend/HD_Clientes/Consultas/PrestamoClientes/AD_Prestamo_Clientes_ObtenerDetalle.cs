using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.PrestamoClientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.PrestamoClientes
{
    public class AD_Prestamo_Clientes_ObtenerDetalle
    {
        private string CadenaConexion;
        public AD_Prestamo_Clientes_ObtenerDetalle(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlPrestamo_Clientes_ObtenerDetalle> Get(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Credito_Prestamo_Obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlPrestamo_Clientes_ObtenerDetalle detalleCliente = new mdlPrestamo_Clientes_ObtenerDetalle();
                detalleCliente.info = result.Read<mdlPrestamo_Cliente_Info>().FirstOrDefault();
                detalleCliente.detallefinanciamiento = result.Read<mdlPedido_Detalle_Financiamiento>().ToList();
                factory.SQL.Close();
                return detalleCliente;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
