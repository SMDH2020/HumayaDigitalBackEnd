using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Guarda_Scorecard_Real_Vendedor
    {
        private string CadenaConexion;
        public AD_Guarda_Scorecard_Real_Vendedor(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdlGuarda_Scorecard_Real_Vendedor>>

            Guardar(mdlGuarda_Scorecard_Real_Vendedor mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    @ejercicio = mdl.ejercicio,
                    @sucursal = mdl.sucursal,
                    @idvendedor = mdl.idvendedor,
                    @usuario = mdl.usuario,
                    @scorecard_detalle = mdl.scorecard_detalle,
                };

                var result = await
                factory.SQL.QueryAsync<mdlGuarda_Scorecard_Real_Vendedor>("Ventas.sp_Scorecard_Real_Vendedor_Guardar",
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
    }
}
