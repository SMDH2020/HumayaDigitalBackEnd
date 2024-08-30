using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos.SolicitudesCerradas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Consultas
{
    public class AD_Solicitudes_Facturadas
    {
        private string CadenaConexion;
        public AD_Solicitudes_Facturadas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdlSolicitudesFacturadasResult> GetSolicitudes(int ejercicio, int periodo, string linea)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo=periodo,
                    linea= linea
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Ventas.sp_Solicitudes_Credito_Facturadas", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlSolicitudesFacturadasResult mdl = new mdlSolicitudesFacturadasResult();
                mdl.resumen = result.Read<mdlSolicitudesFacturada_Resumen>().FirstOrDefault();
                mdl.sucursal = result.Read<mdlSolicitudesFacturadas_Sucursal>().ToList();
                if (mdl.resumen is null) mdl.resumen = new mdlSolicitudesFacturada_Resumen();
                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlOperacionesDetalle>> GetSolicitudesDetalle(int ejercicio, int periodo,int idsucursal, string linea)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    idsucursal = idsucursal,
                    linea = linea
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdlOperacionesDetalle>("Ventas.sp_Solicitudes_Credito_Facturadas_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlOperacionesDetalle>> GetSolicitudesDetalle(int ejercicio, int periodo, int idsucursal, string linea,string card)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = ejercicio,
                    periodo = periodo,
                    idsucursal = idsucursal,
                    linea = linea,
                    card=card
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdlOperacionesDetalle>("Ventas.sp_Resultado_operaciones_card_Detalle", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
