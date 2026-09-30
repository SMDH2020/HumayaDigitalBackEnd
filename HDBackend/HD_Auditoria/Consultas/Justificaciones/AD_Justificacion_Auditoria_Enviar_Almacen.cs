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
    public class AD_Justificacion_Auditoria_Enviar_Almacen
    {
        private string CadenaConexion;
        public AD_Justificacion_Auditoria_Enviar_Almacen(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Notificar_Correo>> Correos(string folio)
        {
            try
            {
                var parametros = new
                {
                    @folio = folio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Notificar_Correo> result = await factory.SQL.QueryAsync<mdl_Notificar_Correo>("Auditoria.SP_JUSTIFICACIONES_INVENTARIO_ENVIAR_ALMACEN", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
