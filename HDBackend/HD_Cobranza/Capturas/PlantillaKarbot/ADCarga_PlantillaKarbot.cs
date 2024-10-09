using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.PlantillaKarbot;

namespace HD_Cobranza.Capturas.PlantillaKarbot
{
    public class ADCarga_PlantillaKarbot
    {
        private string CadenaConexion;
        public ADCarga_PlantillaKarbot(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Carga_PlantillaKarbot>> Plantillas(string linea, string cartera, string telefono)


        {
            try
            {
                var parametros = new
                {
                    linea,
                    cartera,
                    telefono

                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Carga_PlantillaKarbot> result = await factory.SQL.QueryAsync<mdl_Carga_PlantillaKarbot>("Cobranza.sp_Cargar_Plantillas_Karbot", parametros, commandType: System.Data.CommandType.StoredProcedure);
                factory.SQL.Close();
                return result;
            }
            catch (System.Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { Mensaje = ex.Message });
            }
        }
        public async Task<IEnumerable<mdl_Carga_PlantillaKarbotOperacion>> PlantillasOperacion(string linea, string cartera, string telefono)
        {
            try
            {
                var parametros = new
                {
                    linea,
                    cartera,
                    telefono
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Carga_PlantillaKarbotOperacion> result = await factory.SQL.QueryAsync<mdl_Carga_PlantillaKarbotOperacion>("Cobranza.sp_Cargar_Plantillas_Karbot", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
