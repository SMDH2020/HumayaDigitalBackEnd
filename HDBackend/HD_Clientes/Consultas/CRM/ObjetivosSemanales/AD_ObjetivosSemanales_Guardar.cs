using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM.ObjetivosSemanales;
using Newtonsoft.Json;

namespace HD.Clientes.Consultas.CRM.ObjetivosSemanales
{
    public class AD_ObjetivosSemanales_Guardar
    {
        private string CadenaConexion;
        public AD_ObjetivosSemanales_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        /// <summary>
        /// Guarda la captura completa del grid. El detalle viaja al SP como JSON
        /// para desarmarse con OPENJSON y hacer los INSERT/UPDATE correspondientes.
        /// </summary>
        public async Task<bool> Guardar(mdl_ObjetivosSemanales_Guardar mdl)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);
            try
            {
                var parametros = new
                {
                    ejercicio = mdl.ejercicio,
                    periodo = mdl.periodo,
                    json = JsonConvert.SerializeObject(mdl.detalle),
                    usuario = mdl.usuario,
                    comentario=mdl.comentarios
                };
                await factory.SQL.QueryAsync("CRM.sp_ObjetivosSemanales_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                factory.SQL.Close();
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
