using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas
{
    public class AD_Obtener_Solicitudes
    {
        private string CadenaConexion;
        public AD_Obtener_Solicitudes(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Obtener_Solicitudes_View> Listado(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente = idcliente
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);

                var multi = await factory.SQL.QueryMultipleAsync("Ventas.sp_Obtener_Solicitudes_Cliente_Cotizaciones", parametros, commandType: System.Data.CommandType.StoredProcedure);

                var model = new mdl_Obtener_Solicitudes_View
                {
                    Solicitudes = await multi.ReadAsync<mdl_datos_Solicitud>(),
                    Contacto_servicio = await multi.ReadAsync<string>(),
                    Contacto_refacciones = await multi.ReadAsync<string>(),
                    Contacto_ventas = await multi.ReadAsync<string>()
                };

                return model;
                }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
