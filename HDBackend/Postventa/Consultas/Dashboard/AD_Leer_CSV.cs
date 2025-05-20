using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Postventa.Modelos;
using HD.AccesoDatos;

namespace Postventa.Consultas.Dashboard
{
    public class AD_Leer_CSV
    {
        private string CadenaConexion;
        public AD_Leer_CSV(string _cadenaconexion)
        {
            CadenaConexion = _cadenaconexion;
        }

        public async Task ProcesarCSVAsync()
        {
            var path = @"C:\Users\<TuUsuario>\Desktop\csv-ejemplo\warranty.csv";

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                Encoding = System.Text.Encoding.UTF8,
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null,
            };

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<WarrantyMap>();

            var records = csv.GetRecords<mdl_Garantias_CSV>();

            FactoryConection factory = new FactoryConection(CadenaConexion);

            foreach (var record in records)
            {
                var parametros = new DynamicParameters();
                parametros.Add("NúmerosDeSerie", record.NúmerosDeSerie, System.Data.DbType.String);
                parametros.Add("Modelo", record.Modelo, System.Data.DbType.String);
                parametros.Add("Cuentas", record.Cuentas, System.Data.DbType.Int32);
                parametros.Add("Concesionario", record.Concesionario, System.Data.DbType.String);
                parametros.Add("CiudadDeConcesion", record.CiudadDeConcesion, System.Data.DbType.String);
                parametros.Add("NombreDeCliente", record.NombreDeCliente, System.Data.DbType.String);
                parametros.Add("Telefono", record.Telefono, System.Data.DbType.String);
                parametros.Add("Calle1", record.Calle1, System.Data.DbType.String);
                parametros.Add("Calle2", record.Calle2, System.Data.DbType.String);
                parametros.Add("CodigoPostal", record.CodigoPostal, System.Data.DbType.Int16);
                parametros.Add("Ciudad", record.Ciudad, System.Data.DbType.String);
                parametros.Add("Region", record.Region, System.Data.DbType.String);
                parametros.Add("Pais", record.Pais, System.Data.DbType.String);
                parametros.Add("InicioGarantia", record.InicioGarantia, System.Data.DbType.String);
                parametros.Add("Expiracion", record.Expiracion, System.Data.DbType.String);
                parametros.Add("LimiteTiempo", record.LimiteTiempo, System.Data.DbType.String);
                parametros.Add("TipoGarantia", record.TipoGarantia, System.Data.DbType.String);
                parametros.Add("TipoCobertura", record.TipoCobertura, System.Data.DbType.String);
                parametros.Add("ContratoAdquirido", record.ContratoAdquirido);


                await factory.SQL.QueryMultipleAsync("PixelCode.Posventa.sp_dashboard", parametros, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }


    public sealed class WarrantyMap : ClassMap<mdl_Garantias_CSV>
    {
        public WarrantyMap()
        {
            Map(m => m.NúmerosDeSerie).Name("Números de serie ");
            Map(m => m.Modelo).Name("Modelo ");
            Map(m => m.Cuentas).Name("Cuentas ");
            Map(m => m.Concesionario).Name("Concesionario ");
            Map(m => m.CiudadDeConcesion).Name("Ciudad de Concesión");
            Map(m => m.NombreDeCliente).Name("Nombre de Cliente");
            Map(m => m.Telefono).Name("Teléfono");
            Map(m => m.Calle1).Name("Calle 1");
            Map(m => m.Calle2).Name("Calle 2");
            Map(m => m.CodigoPostal).Name("Código postal");
            Map(m => m.Ciudad).Name("Ciudad");
            Map(m => m.Region).Name("Región ");
            Map(m => m.Pais).Name("País");
            Map(m => m.InicioGarantia).Name("Inicio de la Garantía Básica");
            Map(m => m.Expiracion).Name("Expiración");
            Map(m => m.LimiteTiempo).Name("Límite de Tiempo");
            Map(m => m.TipoGarantia).Name("Tipo de Garantía");
            Map(m => m.TipoCobertura).Name("Tipo de cobertura");
            Map(m => m.ContratoAdquirido).Name("Contrato Adquirido");
        }
    }
}
