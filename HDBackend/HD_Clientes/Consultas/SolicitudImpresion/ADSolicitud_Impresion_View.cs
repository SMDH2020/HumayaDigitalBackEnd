using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.Pedido_Impresion;
using HD.Clientes.Modelos.Solicitud_Impresion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Consultas.SolicitudImpresion
{
    public class ADSolicitud_Impresion_View
    {
        private string CadenaConexion;
        public ADSolicitud_Impresion_View(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_Solicitud_Impresion> Get(string folio)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    folio
                };
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Solicitud_Impresion_PDF", parametros, commandType: System.Data.CommandType.StoredProcedure);

                mdl_Solicitud_Impresion impresion = new mdl_Solicitud_Impresion();
                impresion.vendedor = result.Read<mdl_Solicitud_Credito_Vendedor>().FirstOrDefault();
                impresion.datosgenerales = result.Read<mdl_Solicitud_Datos_Generales_View>().FirstOrDefault();
                impresion.contactos = result.Read<mdl_Solicitud_Contactos_View>().ToList();
                impresion.domicilios = result.Read<mdl_Solicitud_Domicilios_View>().ToList();
                impresion.cultivos = result.Read<mdl_Solicitud_Cultivos_View>().ToList();
                impresion.balancepatrimonial = result.Read<mdl_Solicitud_Balance_Patrimonial_View>().FirstOrDefault();
                impresion.estadoresultados = result.Read<mdl_Solicitud_Estado_Resultados_View>().FirstOrDefault();
                impresion.otrosingresos = result.Read<mdl_Solicitud_Otros_Ingresos_View>().ToList();
                impresion.siniestros = result.Read<mdl_Solicitud_Siniestros_View>().ToList();
                factory.SQL.Close();
                return impresion;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
