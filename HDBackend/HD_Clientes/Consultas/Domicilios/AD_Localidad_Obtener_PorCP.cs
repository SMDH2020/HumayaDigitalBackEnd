using Dapper;
using HD.AccesoDatos;
using HD.Clientes.Domicilio;

namespace HD.Clientes.Consultas.Domicilios
{
    public class AD_Localidad_Obtener_PorCP
    {
        private string CadenaConexion;
        public AD_Localidad_Obtener_PorCP(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<mdlLocalidadResult> Listado(string   codigo_postal)
        {
            try
            {
                var parametros = new
                {
                    codigo_postal
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var result = await factory.SQL.QueryMultipleAsync("Credito.sp_Localidades_ObtenerPorCP", parametros, commandType: System.Data.CommandType.StoredProcedure);
                mdlLocalidadResult localidad = new mdlLocalidadResult();
                localidad.estado = result.Read<mdlEstadoMunicipio>().FirstOrDefault();
                localidad.localidades = result.Read<mdlDropDownList>().ToList();

                factory.SQL.Close();
                return localidad;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
