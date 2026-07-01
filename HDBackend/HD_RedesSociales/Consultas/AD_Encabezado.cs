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
    public class AD_Encabezado
    {
        private readonly string CadenaConexion;

        public AD_Encabezado(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task<string> GuardarAsync(mdl_Encabezado encabezado)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new DynamicParameters();

            parametros.Add("@Linea", encabezado.Linea);
            parametros.Add("@Tipo_Publicacion", encabezado.Tipo_Publicacion);
            parametros.Add("@Precio_Lista", encabezado.Precio_Lista);
            parametros.Add("@Firma", encabezado.Firma);
            parametros.Add("@Precio_Especial", encabezado.Precio_Especial);
            parametros.Add("@Beneficios", encabezado.Beneficios);
            parametros.Add("@Vigencias", encabezado.Vigencias);
            parametros.Add("@Restricciones", encabezado.Restricciones);
            parametros.Add("@Escenografia", encabezado.Escenografia);
            parametros.Add("@Hora", encabezado.Hora);
            parametros.Add("@Red_Social", encabezado.Red_Social);

            parametros.Add("@ImagenBase64",
                encabezado.ImagenBase64,
                dbType: System.Data.DbType.String);

            var folioGenerado = await factory.SQL.QueryFirstOrDefaultAsync<string>(
                "HumayaDigital_Eventos.RedesSociales.SP_Encabezado_Guardar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
            return folioGenerado;
        }

        public async Task EditarAsync(mdl_Encabezado encabezado)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var parametros = new
            {
                Folio            = encabezado.Folio,
                Linea            = encabezado.Linea,
                Firma            = encabezado.Firma,    
                Tipo_Publicacion = encabezado.Tipo_Publicacion,
                Precio_Lista     = encabezado.Precio_Lista,
                Precio_Especial  = encabezado.Precio_Especial,
                Beneficios       = encabezado.Beneficios,
                Vigencias        = encabezado.Vigencias,
                Restricciones    = encabezado.Restricciones,
                Escenografia     = encabezado.Escenografia,
                Hora             = encabezado.Hora,
                Red_Social       = encabezado.Red_Social,
                ImagenBase64     = encabezado.ImagenBase64
                

            };

            await factory.SQL.ExecuteAsync(
                "HumayaDigital_Eventos.RedesSociales.SP_Encabezado_Editar",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
        }

        public async Task EliminarAsync(string folio)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            await factory.SQL.ExecuteAsync(
                "HumayaDigital_Eventos.RedesSociales.SP_Encabezado_Eliminar",
                new { Folio = folio },
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
        }

        public async Task<List<mdl_Encabezado>> ListadoAsync(string? folio = null)
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var result = await factory.SQL.QueryAsync<mdl_Encabezado>(
                "HumayaDigital_Eventos.RedesSociales.SP_Encabezado_Listado",
                new { Folio = folio },
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
            return result.ToList();
        }

        

        public async Task<List<mdl_Calendario>> CalendarioAsync()
        {
            FactoryConection factory = new FactoryConection(CadenaConexion);

            var result = await factory.SQL.QueryAsync<mdl_Calendario>(
                "HumayaDigital_Eventos.RedesSociales.SP_Calendario_Listado",
                commandType: System.Data.CommandType.StoredProcedure
            );
            factory.SQL.Close();
            return result.ToList();
        }
    }
}