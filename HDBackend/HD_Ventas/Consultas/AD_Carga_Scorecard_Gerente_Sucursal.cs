using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Carga_Scorecard_Gerente_Sucursal
    {
        private string CadenaConexion;
        public AD_Carga_Scorecard_Gerente_Sucursal(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlCarga_Scorecard_Gerente_Sucursal>> Scorecard(int usuario)
        {
            try
            {
                var parametros = new
                {
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlCarga_Scorecard_Gerente_Sucursal> result = await factory.SQL.QueryAsync<mdlCarga_Scorecard_Gerente_Sucursal>("Ventas.Obtener_Scorecard_GerenteSucursal", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
