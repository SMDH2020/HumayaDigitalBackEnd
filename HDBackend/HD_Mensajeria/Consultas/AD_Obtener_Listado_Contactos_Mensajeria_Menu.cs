using Dapper;
using HD.AccesoDatos;
using HD_Mensajeria.Modelos;

namespace HD_Mensajeria.Consultas
{
    public class AD_Obtener_Listado_Contactos_Mensajeria_Menu
    {
        private string CadenaConexion;
        public AD_Obtener_Listado_Contactos_Mensajeria_Menu(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<mdl_Contactos_Mensajeria_View> obtenerContactos(int idusuario)
        {
            try
            {
                var parametros = new
                {
                    idusuario = idusuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("HD_Mensajeria.dbo.sp_Obtener_Listado_Clientes_Contactados_3", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Contactos_Mensajeria_View mdl = new mdl_Contactos_Mensajeria_View();
                mdl.postventa = result.Read<mdl_Contactos_Mensajeria_Menu>().ToList();
                mdl.cobranza = result.Read<mdl_Contactos_Mensajeria_Menu>().ToList();
                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Mensajeria_Indicadores_View> obtenerIndicadores(string fechainicio, string fechafin, string? linea, string? adr, string? sucursal, string? plantilla)
        {
            try
            {
                var parametros = new
                {
                    FechaInicio = fechainicio,
                    FechaFin = fechafin,
                    Linea = linea,
                    Adr = adr,
                    Sucursales = sucursal,
                    Plantilla = plantilla
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("HD_Mensajeria.dbo.sp_Obtener_Indicadores_Respuesta_Mensajeria", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Mensajeria_Indicadores_View mdl = new mdl_Mensajeria_Indicadores_View();
                mdl.header = result.Read<mdl_Mensajeria_Indicadores_Header>().FirstOrDefault();
                mdl.masRespuestas = result.Read<mdl_Mensajeria_Indicadores_Top>().ToList();
                mdl.menosRespuestas = result.Read<mdl_Mensajeria_Indicadores_Top>().ToList();
                mdl.listado = result.Read<mdl_Mensajeria_Indicadores_Detalle>().ToList();
                factory.SQL.Close();
                return mdl;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
