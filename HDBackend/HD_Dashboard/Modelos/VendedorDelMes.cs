using Dapper;
using ExcelDataReader;
using HD.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Dashboard.Modelos
{
    internal class VendedorDelMes
    {
        public string nombre { get; set; }
        public double utilidad { get; set; }

        public int mes { get; set; }
        public int año { get; set; }

        public static List<VendedorDelMes> ObtenerVendedorDelMesExcel()
        {
            var filePath = "C:\\SDMH\\HumayaDigital\\CONTROL DE FACTURACION.xls";
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
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
                    var date = DateTime.Now.AddMonths(-2);

                    //fecha de inicio y fecha de fin del mes anterior
                    var startDate = new DateTime(date.Year, date.Month, 1);
                    var endDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59);
                    //Console.WriteLine(startDate);
                    //Console.WriteLine(endDate);


                    //utilizando Linq para agrupar por nombre del vendedor y sumar sus ventas totales
                    var query = from row in table.AsEnumerable()
                                where row.Field<DateTime>("FECHA") <= endDate && row.Field<DateTime>("FECHA") >= startDate
                                group row by row.Field<string>("VENDEDOR") into g
                                select new VendedorDelMes
                                {
                                    nombre = g.Key,
                                    utilidad = g.Sum(r => r.Field<double>("UTILIDAD FINAL")),
                                    año = date.Year,
                                    mes = date.Month,

                                };


                    query = query.OrderByDescending(p => p.utilidad).ToList();

                    // Mostrar el resultado de la consulta
                    /*
                    foreach (var item in query)
                    {
                        Console.WriteLine("Nombre: {0}, Ventas: {1}", item.Nombre, item.Precio);
                    }
                    Console.ReadLine();
                    */
                    return (List<VendedorDelMes>)query;
                }


            }
        }

        public static bool ObtenerVendedorDelMesExcel(FactoryConection factory)
        {

            try
            {
                int posisicion = 1;
                foreach (var item in VendedorDelMes.ObtenerVendedorDelMesExcel())
                {
                    Console.WriteLine(factory.Mensaje);
                    //Console.WriteLine("Nombre: {0}, Ventas: {1}", item.Nombre, item.Precio);
                    var ultimoEspacio = item.nombre.LastIndexOf(" ");
                    var nombre = item.nombre.Substring(0, ultimoEspacio);
                    var apellido = item.nombre.Substring(ultimoEspacio + 1);

                    var id = factory.SQL.QueryFirstOrDefault<int>("dashboard.sp_Obtener_ID_Vendedor_Por_Nombre", new { nombre = nombre, apellidopaterno = apellido },
                        commandType: CommandType.StoredProcedure);

                    _ = factory.SQL.Query("dashboard.sp_vendedor_Mes_Guardar", new
                    {
                        idvendedormes = -1,
                        ejercicio = item.año,
                        periodo = item.mes,
                        posicion = posisicion++,
                        idempleado = id,
                        estatus = 1,
                        usuario = 0
                    }, commandType: CommandType.StoredProcedure);


                }
                //Console.ReadLine();
                return true;
            }
            catch (SqlException ex)
            {
                // factory.transaccion.Rollback();
                throw ex;
            }
        }
    }
}
