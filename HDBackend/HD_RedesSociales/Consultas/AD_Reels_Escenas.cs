using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HD.AccesoDatos;
using HD_RedesSociales.Modelos;

namespace HD_RedesSociales.Consultas
{
    public class AD_Reels_Escenas
    {
        private readonly string CadenaConexion;

        public AD_Reels_Escenas(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task GuardarAsync(mdl_Reels_Escena escena)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                Folio = escena.Folio,
                Numero_Escena = escena.Numero_Escena,
                Avatar_Id = escena.Avatar_Id,
                Texto = escena.Texto
            };

            await factory.SQL.ExecuteAsync(
                "HumayaDigital_Eventos.RedesSociales.SP_Reels_Escenas_Guardar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
        }

        public async Task EliminarTodasAsync(string folio)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            await factory.SQL.ExecuteAsync(
                "HumayaDigital_Eventos.RedesSociales.SP_Reels_Escenas_EliminarTodas",
                new { Folio = folio },
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
        }

        public async Task<List<mdl_Reels_Escena>> ListadoAsync(string folio)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var result = await factory.SQL.QueryAsync<mdl_Reels_Escena>(
                "HumayaDigital_Eventos.RedesSociales.SP_Reels_Escenas_Listado",
                new { Folio = folio },
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
            return result.ToList();
        }
    }
}