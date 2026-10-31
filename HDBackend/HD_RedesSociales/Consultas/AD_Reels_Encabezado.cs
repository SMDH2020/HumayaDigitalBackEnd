using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HD.AccesoDatos;
using HD_RedesSociales.Modelos;

namespace HD_RedesSociales.Consultas
{
    public class AD_Reels_Encabezado
    {
        private readonly string CadenaConexion;

        public AD_Reels_Encabezado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<string> GuardarAsync(mdl_Reels_Encabezado reel)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                Modo = reel.Modo,
                Tema = reel.Tema,
                Informacion_Empresa = reel.Informacion_Empresa,
                Avatar_Id = reel.Avatar_Id,
                Aspect_Ratio = reel.Aspect_Ratio,
                Width = reel.Width,
                Height = reel.Height,
                Hora = reel.Hora,
                Red_Social = reel.Red_Social,
                Cargar = reel.Cargar
            };

            var folioGenerado = await factory.SQL.QueryFirstOrDefaultAsync<string>(
                "HumayaDigital_Eventos.RedesSociales.SP_Reels_Encabezado_Guardar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
            return folioGenerado;
        }

        public async Task EditarAsync(mdl_Reels_Encabezado reel)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                Folio = reel.Folio,
                Modo = reel.Modo,
                Tema = reel.Tema,
                Informacion_Empresa = reel.Informacion_Empresa,
                Avatar_Id = reel.Avatar_Id,
                Aspect_Ratio = reel.Aspect_Ratio,
                Width = reel.Width,
                Height = reel.Height,
                Hora = reel.Hora,
                Red_Social = reel.Red_Social,
                Cargar = reel.Cargar
            };

            await factory.SQL.ExecuteAsync(
                "HumayaDigital_Eventos.RedesSociales.SP_Reels_Encabezado_Editar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
        }

        public async Task EliminarAsync(string folio)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            await factory.SQL.ExecuteAsync(
                "HumayaDigital_Eventos.RedesSociales.SP_Reels_Encabezado_Eliminar",
                new { Folio = folio },
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
        }

        public async Task<List<mdl_Reels_Encabezado>> ListadoAsync(string? folio = null)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var result = await factory.SQL.QueryAsync<mdl_Reels_Encabezado>(
                "HumayaDigital_Eventos.RedesSociales.SP_Reels_Encabezado_Listado",
                new { Folio = folio },
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
            return result.ToList();
        }
    }
}