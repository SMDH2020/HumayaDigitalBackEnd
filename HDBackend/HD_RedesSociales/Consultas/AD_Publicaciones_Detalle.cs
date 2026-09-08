using Dapper;
using HD.AccesoDatos;
using HD_RedesSociales.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HD.AccesoDatos;
using HD_RedesSociales.Modelos;

namespace HD_RedesSociales.Consultas
{
    public class AD_Publicaciones_Detalle
    {
        private readonly string CadenaConexion;

        public AD_Publicaciones_Detalle(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<int> GuardarAsync(mdl_Publicaciones_Detalle detalle)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                Folio = detalle.Folio,
                Fecha_Envio = detalle.Fecha_Envio,
                Estatus = detalle.Estatus
            };

            var consecutivo = await factory.SQL.QueryFirstOrDefaultAsync<int>(
                "HumayaDigital_Eventos.RedesSociales.SP_Publicaciones_Detalle_Guardar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
            return consecutivo;
        }

        public async Task EditarAsync(mdl_Publicaciones_Detalle detalle)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                Folio = detalle.Folio,
                Consecutivo = detalle.Consecutivo,
                Fecha_Envio = detalle.Fecha_Envio,
                Estatus = detalle.Estatus
            };

            await factory.SQL.ExecuteAsync(
                "HumayaDigital_Eventos.RedesSociales.SP_Publicaciones_Detalle_Editar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
        }

        public async Task EliminarAsync(string folio, int? consecutivo = null)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            await factory.SQL.ExecuteAsync(
                "HumayaDigital_Eventos.RedesSociales.SP_Publicaciones_Detalle_Eliminar",
                new { Folio = folio, Consecutivo = consecutivo },
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
        }

        public async Task<List<mdl_Publicaciones_Detalle>> ListadoAsync(string? folio = null, int? consecutivo = null)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var result = await factory.SQL.QueryAsync<mdl_Publicaciones_Detalle>(
                "HumayaDigital_Eventos.RedesSociales.SP_Publicaciones_Detalle_Listado",
                new { Folio = folio, Consecutivo = consecutivo },
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
            return result.ToList();
        }
    }
}