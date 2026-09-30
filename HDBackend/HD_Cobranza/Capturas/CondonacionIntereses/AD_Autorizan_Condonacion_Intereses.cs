using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.CondonacionIntereses;
using HD_Cobranza.Modelos.Dashboard;
using HD_Cobranza.Modelos.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Capturas.CondonacionIntereses
{
    public class AD_Autorizan_Condonacion_Intereses
    {
        private string CadenaConexion;
        public AD_Autorizan_Condonacion_Intereses(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Guarda_Usuarios_Autorizan_Condonacion>> Guardar(mdl_Guarda_Usuarios_Autorizan_Condonacion mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @id = mdl.id,
                    @usuario_autoriza = mdl.usuario_autoriza,
                    @porcentaje_normal = mdl.porcentaje_normal,
                    @porcentaje_moratorio = mdl.porcentaje_moratorio,
                    @Notificar = mdl.Notificar,
                    @usuario = mdl.usuario,
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Guarda_Usuarios_Autorizan_Condonacion>("Cartera_Clientes.CondonacionInteres.sp_Guardar_Usuarios_Aprueban_Condonaciones",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }

        }

        public async Task<IEnumerable<mdl_Elimina_Usuario_Autoriza_Condonacion>> Eliminar(mdl_Elimina_Usuario_Autoriza_Condonacion mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @id = mdl.id,
                    @usuario = mdl.usuario,
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Elimina_Usuario_Autoriza_Condonacion>("Cartera_Clientes.CondonacionInteres.sp_Eliminar_Usuarios_Aprueban_Condonaciones",
                parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new
                Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }

        }

        public async Task<mdl_Obtener_Usuario_Condonacion_ID_View> ObtenerUsuarioCondonacionID(int idAutoriza)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                //var parametros = new
                //{
                //    ejercicio,
                //    periodo
                //};
                var parametros = new DynamicParameters();
                parametros.Add("idAutoriza", idAutoriza, System.Data.DbType.Int16);

                var result = await factory.SQL.QueryMultipleAsync("Cartera_Clientes.CondonacionInteres.sp_Obtener_Usuario_Condonacion_Interes_ID", parametros, commandType: System.Data.CommandType.StoredProcedure);
                var view = new mdl_Obtener_Usuario_Condonacion_ID_View();
                view.usuarios = result.Read<mdl_DDL_Usuarios_Condonacion_Intereses>().ToList();
                view.infoUsuarioCondonacion = result.Read<mdl_Obtener_Usuario_Condonacion_ID>().FirstOrDefault();
                factory.SQL.Close();
                return view;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Obtener_Listado_Usuarios_Condonacion>> ListadoUsuarios()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryAsync<mdl_Obtener_Listado_Usuarios_Condonacion>("Cartera_Clientes.CondonacionInteres.sp_Obtener_Listado_Usuarios_Condonacion_Interes", commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { mensaje = ex.Message });
            }
        }
    }
}
