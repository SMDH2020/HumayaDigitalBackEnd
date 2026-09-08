using Dapper;
using HD.AccesoDatos;
using Ventas.Modelos;

namespace Ventas.Consultas
{
    public class AD_LineasScorecard_Guardar
    {
        private string CadenaConexion;
        public AD_LineasScorecard_Guardar(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<bool> Guardar(mdl_LineasScorecard mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    idlinea = mdl.idlinea,
                    descripcion = mdl.descripcion,
                    estatus = mdl.estatus,
                    usuario = mdl.usuario
                };
                await factory.SQL.QueryAsync("Ventas.sp_Lineas_Scorecard_Guardar", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
    }
}
