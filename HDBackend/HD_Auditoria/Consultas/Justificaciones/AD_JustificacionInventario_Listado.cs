using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Justificaciones
{
    public class AD_JustificacionInventario_Listado
    {
        private string CadenaConexion;
        public AD_JustificacionInventario_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_JustificacionInventario_Listado>> Listado(string folio)
        {
            try
            {
                var parametros = new
                {
                    @folio = folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_JustificacionInventario_Listado> result = await factory.SQL.QueryAsync<mdl_JustificacionInventario_Listado>("Auditoria.SP_JUSTIFICACIONES_INVENTARIO_LISTADO", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
