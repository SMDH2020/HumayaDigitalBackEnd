using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;
using HD_Cobranza.Modelos.PlantillaKarbot;

namespace HD_Cobranza.Capturas.PlantillaKarbot
{
    public class ADGuarda_PlantillasKarbot
    {
        private string CadenaConexion;
        public ADGuarda_PlantillasKarbot(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }
        public async Task<IEnumerable<mdl_Guarda_PlantillasKarbot>>Guardar(mdl_Guarda_PlantillasKarbot mdl)
        {
            try
            {
                FactoryConection factory = new FactoryConection(CadenaConexion);
                var parametros = new
                {
                    cartera=mdl.cartera,
                    linea=mdl.linea,
                    @listado_plantillas = mdl.listado_plantillas,
                    @usuario = mdl.usuario
                };

                var result = await
                factory.SQL.QueryAsync<mdl_Guarda_PlantillasKarbot>("Cobranza.sp_Guardar_Plantillas_Karbot_Aprobadas",
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
