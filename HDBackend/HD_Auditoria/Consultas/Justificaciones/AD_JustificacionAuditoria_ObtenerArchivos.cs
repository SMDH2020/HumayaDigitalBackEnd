using Dapper;
using HD.AccesoDatos;
using HD_Auditoria.Modelos.Justificaciones;
using HD_Auditoria.Modelos.Programar_Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Consultas.Justificaciones
{
    public class AD_JustificacionAuditoria_ObtenerArchivos
    {
        private string CadenaConexion;
        public AD_JustificacionAuditoria_ObtenerArchivos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_ObtenerArchivos>> obtenerArchivos(int idjust)
        {
            try
            {
                var parametros = new
                {
                    @idjust = idjust
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_ObtenerArchivos> result = await factory.SQL.QueryAsync<mdl_ObtenerArchivos>("Auditoria.SP_JUST_AUDITORIA_OBTENER_ARCHIVOS", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
