using ExcelDataReader;
using System.Collections.Generic;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace DesktopServices.VendedorMes
{
    public class LeerFacturacionMensual
    {
        public static List<mdlVendedorMes> ObtenerVendedorDelMesExcel(string path)
        {    
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                // Crear un lector de Excel con la fábrica
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Crear una configuración para el DataSet
                    var config = new ExcelDataSetConfiguration()
                    {
                        // Indicar que se usen los nombres de las columnas del archivo
                        UseColumnDataType = true,

                        // Indicar que se omitan las primeras 7 filas antes de leer los nombres de las columnas
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true,
                            ReadHeaderRow = (rowReader) =>
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    rowReader.Read();
                                }
                            },
                            FilterRow = (row) =>
                            {
                                return row[0] != null;
                            }

                        },


                    };

                    // Convertir el lector en un DataSet
                    var result = reader.AsDataSet(config);
                    var table = result.Tables[0];

                    //fecha del mes anterior
                    var date = DateTime.Now.AddMonths(-1);

                    //fecha de inicio y fecha de fin del mes anterior
                    var startDate = new DateTime(date.Year, date.Month, 1);
                    var endDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
                    //Console.WriteLine(startDate);
                    //Console.WriteLine(endDate);


                    //utilizando Linq para agrupar por nombre del vendedor y sumar sus ventas totales
                    var query = from row in table.AsEnumerable()
                                where row.Field<DateTime>("FECHA") <= endDate && row.Field<DateTime>("FECHA") >= startDate
                                group row by row.Field<string>("VENDEDOR") into g
                                select new mdlVendedorMes
                                {
                                    nombre = g.Key,
                                    utilidad = g.Sum(r => r.Field<double>("UTILIDAD FINAL")),
                                    año = date.Year,
                                    mes = date.Month,

                                };


                    query = query.OrderByDescending(p => p.utilidad).ToList();
                    return (List<mdlVendedorMes>)query;
                }


            }
        }
    }
}
