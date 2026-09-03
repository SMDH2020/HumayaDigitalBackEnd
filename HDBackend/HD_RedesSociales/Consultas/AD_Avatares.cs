using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HD.AccesoDatos;
using HD_RedesSociales.Modelos;

namespace HD_RedesSociales.Consultas
{
    public class AD_Avatares
    {
        private readonly string CadenaConexion;

        public AD_Avatares(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<List<mdl_Avatar>> ListadoAsync(bool? activo = null)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var result = await factory.SQL.QueryAsync<mdl_Avatar>(
                "HumayaDigital_Eventos.RedesSociales.SP_Avatares_Listado",
                new { Activo = activo },
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
            return result.ToList();
        }
    }
}