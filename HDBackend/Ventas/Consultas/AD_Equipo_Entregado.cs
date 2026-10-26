using HD.AccesoDatos;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas.Modelos.EquipoEntregado;
using Dapper;

namespace Ventas.Consultas
{
    public class AD_Equipo_Entregado
    {
        private string CadenaConexion;
        public AD_Equipo_Entregado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }


        public async Task<mdl_equipo_entregado_view> ObtenerInfo(
            string folio_cotizacion, int orden_cotizacion, int idcliente)
        {
            try
            {
                var parametros = new
                {
                    folio_cotizacion,
                    orden_cotizacion,
                    idcliente
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync(
                    "Ventas.sp_Obtener_Equipo_Entregado_Info",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);

                mdl_equipo_entregado_view mdl = new mdl_equipo_entregado_view();
                mdl.entrega = result.Read<mdl_equipo_entregado>().FirstOrDefault();
                mdl.solicitud = result.Read<mdl_Solicitud>().FirstOrDefault();
                mdl.solicitudes_cliente = result.Read<mdl_Solicitud>().ToList();

                if (mdl.entrega is null) mdl.entrega = new mdl_equipo_entregado();

                factory.SQL.Close();
                return mdl;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Solicitud> ObtenerSolicitudByFolio(string folio_solicitud)
        {
            try
            {
                var parametros = new { folio_solicitud };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdl_Solicitud>(
                    "Ventas.sp_Buscador_Solicitudes_Credito",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }

        public async Task<bool> GuardarEquipoEntregado(mdl_Guardar_Equipo_Entregado mdl)
        {
            try
            {
                var parametros = new
                {
                    folio_cotizacion = mdl.folio_cotizacion,
                    orden_cotizacion = mdl.orden_cotizacion,
                    comentarios = mdl.comentarios,
                    fecha_entrega = mdl.fecha_entrega,
                    folio_solicitud = mdl.folio_solicitud,
                    usuario = mdl.usuario
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync(
                    "Ventas.sp_Guardar_Equipo_Entregado",
                    parametros,
                    commandType: System.Data.CommandType.StoredProcedure);

                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError,
                    new { Mensaje = ex.Message });
            }
        }
    }
}
