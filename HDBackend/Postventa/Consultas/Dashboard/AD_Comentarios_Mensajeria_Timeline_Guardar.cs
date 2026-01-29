using Dapper;
using HD.AccesoDatos;
using Postventa.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Consultas.Dashboard
{
    public class AD_Comentarios_Mensajeria_Timeline_Guardar
    {
        private string CadenaConexion;
        public AD_Comentarios_Mensajeria_Timeline_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
       
        public async Task<IEnumerable<mdl_Comentarios_Timeline_Mensajeria_Listado>> guardar(mdl_Comentarios_Timeline_Mensajeria_Guardar mdl) //no se utiliza
        {

            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idcomentario = mdl.idcomentario,
                    tipo = mdl.tipo,
                    tipo_contacto = mdl.tipo_contacto,
                    fase = mdl.fase,
                    recordatorio = mdl.recordatorio,
                    comentario = mdl.comentario,
                    horometro = mdl.horometro,
                    documento_factura = mdl.documento_factura,
                    usuario = mdl.usuario,
                };
                IEnumerable<mdl_Comentarios_Timeline_Mensajeria_Listado> result = await factory.SQL.QueryAsync<mdl_Comentarios_Timeline_Mensajeria_Listado>("Postventa.sp_Comentarios_Timeline_Mensajeria_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
