using Dapper;
using HD.AccesoDatos;
using HD_Ventas.Modelos;

namespace HD_Ventas.Consultas
{
    public class AD_Modifica_Scorecard_Objetivo
    {
        private string CadenaConexion;
        public AD_Modifica_Scorecard_Objetivo(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdlModifica_Scorecard_Objetivo>> Modifica(mdlModifica_Scorecard_Objetivo mdl)
        {
            try
            {
                var parametros = new
                {
                    ejercicio = mdl.ejercicio,
                    usuario = mdl.usuario,
                    vendedor = mdl.idvendedor
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdlModifica_Scorecard_Objetivo> result = await factory.SQL.QueryAsync<mdlModifica_Scorecard_Objetivo>("Ventas.sp_Scorecard_Modificar_Objetivo", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
