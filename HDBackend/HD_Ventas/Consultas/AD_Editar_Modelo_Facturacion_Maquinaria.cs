using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Editar_Modelo_Facturacion_Maquinaria
    {
        private string CadenaConexion;
        public AD_Editar_Modelo_Facturacion_Maquinaria(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlListado_Facturacion_Maquinaria>> Editar(int id, string nip, string modelo, string usuario)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    id = id,
                    nip = nip,
                    modelo = modelo,
                    usuario = usuario
                };
                IEnumerable<mdlListado_Facturacion_Maquinaria> result = await factory.SQL.QueryAsync<mdlListado_Facturacion_Maquinaria>("Ventas.sp_Editar_Modelo_Facturacion_Maquinaria", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
