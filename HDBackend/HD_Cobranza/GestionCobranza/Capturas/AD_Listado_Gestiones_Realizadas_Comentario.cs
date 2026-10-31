using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.GestionCobranza.Modelos;

namespace HD_Cobranza.GestionCobranza.Capturas
{
    public class AD_Listado_Gestiones_Realizadas_Comentario
    {
        private string CadenaConexion;
        public AD_Listado_Gestiones_Realizadas_Comentario(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Listado_Gestiones_Realizadas_Comentario>> Get(string? fechainicio, string? fechafin, string adr, string sucursal, int responsable, int objecion)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    fechainicio = fechainicio,
                    fechafin = fechafin,
                    adr = adr,
                    sucursal = sucursal,
                    responsable = responsable,
                    objecion = objecion
                    //rango = rango

                };
                IEnumerable<mdl_Listado_Gestiones_Realizadas_Comentario> result = await factory.SQL.QueryAsync<mdl_Listado_Gestiones_Realizadas_Comentario>("GestionCobranza.sp_Listado_Gestiones_Realizadas_porRango", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }

        public async Task<IEnumerable<mdl_Objeciones_DropDownList>> Objeciones()
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                };
                IEnumerable<mdl_Objeciones_DropDownList> result = await factory.SQL.QueryAsync<mdl_Objeciones_DropDownList>("Cobranza.sp_Get_Objeciones_DropDownList", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
