using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Simulador_Incentivos
    {
        private string CadenaConexion;
        public AD_Simulador_Incentivos(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlSimulador_Roles_Usuario>> Obtener_Roles_Usuario(int usuario)
        {
            try
            {
                var parametros = new
                {
                    idusuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlSimulador_Roles_Usuario> result = await factory.SQL.QueryAsync<mdlSimulador_Roles_Usuario>("sp_obtener_roles_simulador", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<object> Guardar_Documento(mdlSimulador_documento_guardar obj)
        {
            try
            {
                var parametros = new
                {
                    idrol=obj.idrol,
                    documento=obj.idrol,
                    idusuario = obj.usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                await factory.SQL.QueryAsync<mdlSimulador_documento_obtener>("sp_simulador_incentivo_documento_guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return new
                {
                    mensaje="Documento cargado con exito"
                };
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdlSimulador_documento_obtener>> Obtener_Documento(string idrol)
        {
            try
            {
                var parametros = new
                {
                    idrol = idrol
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlSimulador_documento_obtener> result = await factory.SQL.QueryAsync<mdlSimulador_documento_obtener>("sp_simulador_incentivo_documento_obtener", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
