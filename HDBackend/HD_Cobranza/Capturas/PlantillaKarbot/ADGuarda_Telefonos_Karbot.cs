using Dapper;
using HD.AccesoDatos;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.PlantillaKarbot;

namespace HD_Cobranza.Capturas.PlantillaKarbot
{
    public class ADGuarda_Telefonos_Karbot
    {
        private string CadenaConexion;
        public ADGuarda_Telefonos_Karbot(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<IEnumerable<mdl_Carga_PlantillaKarbot>> Contacto(int idcliente, string telefono, int usuario)
        {
            try
            {
                var parametros = new
                {
                    idcliente = idcliente,
                    valor = telefono,
                    usuario = usuario
                };
                FactoryConection factory = new FactoryConection(CadenaConexion);
                IEnumerable<mdl_Carga_PlantillaKarbot> result = await factory.SQL.QueryAsync<mdl_Carga_PlantillaKarbot>("Cobranza.sp_Guardar_Telefono_Plantillas_Karbot", parametros, commandType: System.Data.CommandType.StoredProcedure);
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
