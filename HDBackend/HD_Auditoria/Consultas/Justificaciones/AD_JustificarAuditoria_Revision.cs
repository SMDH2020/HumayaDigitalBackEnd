using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Conteo_Piezas;
using HD_Auditoria.Modelos.Justificaciones;
using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Justificaciones
{
    public class AD_JustificarAuditoria_Revision
    {
        private string CadenaConexion;
        public AD_JustificarAuditoria_Revision(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdl_AuditoriaJustificacionesListado_View> Listado(string folio)
        {
            try
            {
                var parametros = new
                {
                    @folio = folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Auditoria.SP_JUSTIFICACIONES_INVENTARIO_LISTADO", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_AuditoriaJustificacionesListado_View listado = new mdl_AuditoriaJustificacionesListado_View();
                listado.header = result.Read<mdl_Listado_Inventario_Conteo_Header>().FirstOrDefault();
                listado.Listado = result.Read<mdl_JustificacionInventario_Listado>().ToList();
                factory.SQL.Close();
                return listado;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
