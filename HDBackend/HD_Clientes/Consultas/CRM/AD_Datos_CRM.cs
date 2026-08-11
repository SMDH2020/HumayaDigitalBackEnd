using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Modelos.CRM;

namespace HD.Clientes.Consultas.CRM
{
    public class AD_Datos_CRM
    {
        private string CadenaConexion;
        public AD_Datos_CRM(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_Clientes_CRM>> Listado()
        {
            try
            {
                var parametros = new
                {
                    
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Listado_Clientes_CRM> result = await factory.SQL.QueryAsync<mdl_Listado_Clientes_CRM>("Credito.sp_Get_Clientes_CRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<mdl_Info_Cliente_CRM_ID_View> Obtener_Info_Cliente(int idcliente)
        {
            try
            {
                var parametros = new
                {
                    idcliente = idcliente
                };

                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("CRM.sp_Get_Info_Cliente_CRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdl_Info_Cliente_CRM_ID_View mdl = new mdl_Info_Cliente_CRM_ID_View();
                mdl.info_General_cliente = result.Read<mdl_Info_Cliente_CRM>().FirstOrDefault();
                mdl.opciones_estatus = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_origen = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.opciones_tipo = result.Read<mdl_Opciones_Generales_CRM>().ToList();
                mdl.info_ubicacion_cliente = result.Read<mdl_Info_Cliente_Ubicacion_CRM>().ToList();
                mdl.opciones_estado = result.Read<mdl_Opciones_Estado_CRM>().ToList();
                mdl.opciones_municipio = result.Read<mdl_Opciones_Municipio_CRM>().ToList();

                factory.SQL.Close();
                return mdl;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Opciones_Localidades_CRM>> Listado_localidades(string codigo_postal = null, int? idmunicipio = null)
        {
            try
            {
                var parametros = new
                {
                    codigo_postal = codigo_postal,
                    idmunicipio = idmunicipio
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Opciones_Localidades_CRM> result = await factory.SQL.QueryAsync<mdl_Opciones_Localidades_CRM>("CRM.sp_Get_Localidades_CRM", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
