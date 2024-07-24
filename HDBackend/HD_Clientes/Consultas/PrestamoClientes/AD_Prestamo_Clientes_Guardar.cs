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
    public class AD_Prestamo_Clientes_Guardar
    {
        private string CadenaConexion;
        public AD_Prestamo_Clientes_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlPrestamo_Clientes_ObtenerDetalle> GuardarRel(mdlPrestamo_Clientes_Guardar mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio = mdl.folio,
                    monto = mdl.monto,
                    usuario = mdl.usuario
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Crear_Solicitud_Credito_Prestamos", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlPrestamo_Clientes_ObtenerDetalle clienteDetalle = new mdlPrestamo_Clientes_ObtenerDetalle();
                clienteDetalle.info = result.Read<mdlPrestamo_Cliente_Info>().FirstOrDefault();
                clienteDetalle.detallefinanciamiento = result.Read<mdlPedido_Detalle_Financiamiento>().ToList();
                factory.SQL.Close();
                return clienteDetalle;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
