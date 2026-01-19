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
    public class AD_Comentario_Timeline_Mensajeria_Listado
    {
        private string CadenaConexion;
        public AD_Comentario_Timeline_Mensajeria_Listado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Comentarios_Timeline_Mensajeria_Listado>> Listado(int idcomentario, string tipo)
        {
            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("idcomentario", idcomentario, System.Data.DbType.Int32);
                parametros.Add("tipo", tipo, System.Data.DbType.String);

                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Comentarios_Timeline_Mensajeria_Listado> result = await factory.SQL.QueryAsync<mdl_Comentarios_Timeline_Mensajeria_Listado>("Postventa.sp_Comentarios_Timeline_Mensajeria_Listado", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
