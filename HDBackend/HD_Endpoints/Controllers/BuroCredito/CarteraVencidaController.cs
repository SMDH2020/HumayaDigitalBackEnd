using DocumentFormat.OpenXml.Spreadsheet;
using HD.Clientes.Consultas.Clientes;
using HD.Clientes.Modelos;
using HD.Security;
using HD_Buro.Consultas;
using HD_Buro.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace HD.Endpoints.Controllers.BuroCredito
{
    public class CarteraVencidaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CarteraVencidaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        private DateTime CorregirFecha(string value)
        {
            try
            {
                string val_date = value.Substring(2, 1);

                if (val_date != "/") value = string.Concat("0", value);

                val_date = value.Substring(5, 1);

                if (val_date != "/") value = string.Concat(value.Substring(0, 3), "0", value.Substring(3));

                string newvalue = "";

                newvalue += value.Substring(3, 2);
                newvalue += "/";
                newvalue += value.Substring(0, 2);
                newvalue += "/";
                newvalue += value.Substring(6, 4);

                return DateTime.Parse(newvalue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cartera_Vencida_Guardar datos = new AD_Cartera_Vencida_Guardar(CadenaConexion);

            try
            {
                string rutaArchivo = @"C:\Users\PracticanteSistema\Desktop\Cargar Cobranza\Buro\CarteraVencida.csv";
                string separador = ",";

                using (var reader = new StreamReader(rutaArchivo))
                {
                    // Saltar la primera línea (encabezados)
                    reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        var linea = await reader.ReadLineAsync();
                        var valores = linea.Split(separador);

                        // Utilizar expresión regular para extraer los números antes del guion medio
                        string clienteString = Regex.Match(valores[0], @"\d+(?=\s*-)").Value;

                        // Convertir el string obtenido a un entero de manera segura
                        long cliente;
                        if (!string.IsNullOrEmpty(clienteString))
                        {
                            cliente = Convert.ToInt64(clienteString);
                        }
                        else
                        {
                            cliente = 0;
                        }

                        // Utilizar la función CorregirFecha para formatear la fecha
                        DateTime fechaCorregida = CorregirFecha(valores[1]);

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var telefonoCelNumerico = Regex.Replace(valores[3], "[^0-9]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        long telefonoCel;
                        if (!string.IsNullOrEmpty(telefonoCelNumerico))
                        {
                            telefonoCel = Convert.ToInt64(telefonoCelNumerico);
                        }
                        else
                        {
                            telefonoCel = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var sucursalNumerico = Regex.Replace(valores[4], "[^0-9]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        short sucursal;
                        if (!string.IsNullOrEmpty(sucursalNumerico))
                        {
                            sucursal = Convert.ToInt16(sucursalNumerico);
                        }
                        else
                        {
                            sucursal = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var valororigenNumerico = Regex.Replace(valores[7], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double valororigen;
                        if (!string.IsNullOrEmpty(valororigenNumerico))
                        {
                            valororigen = Convert.ToDouble(valororigenNumerico);
                        }
                        else
                        {
                            valororigen = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var regNumerico = Regex.Replace(valores[8], "[^0-9.]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        int reg;
                        if (!string.IsNullOrEmpty(regNumerico))
                        {
                            reg = Convert.ToInt32(regNumerico);
                        }
                        else
                        {
                            reg = 0;
                        }


                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var pagadoNumerico = Regex.Replace(valores[9], "[^0-9.]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double pagado;
                        if (!string.IsNullOrEmpty(pagadoNumerico))
                        {
                            pagado = Convert.ToDouble(pagadoNumerico);
                        }
                        else
                        {
                            pagado = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var saldoNumerico = Regex.Replace(valores[10], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double saldo;
                        if (!string.IsNullOrEmpty(saldoNumerico))
                        {
                            saldo = Convert.ToDouble(saldoNumerico);
                        }
                        else
                        {
                            saldo = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var creditoNumerico = Regex.Replace(valores[11], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double credito;
                        if (!string.IsNullOrEmpty(creditoNumerico))
                        {
                            credito = Convert.ToDouble(creditoNumerico);
                        }
                        else
                        {
                            credito = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var terminocred2Numerico = Regex.Replace(valores[12], "[^0-9]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        short terminocred2;
                        if (!string.IsNullOrEmpty(terminocred2Numerico))
                        {
                            terminocred2 = Convert.ToInt16(terminocred2Numerico);
                        }
                        else
                        {
                            terminocred2 = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var terminocred1Numerico = Regex.Replace(valores[13], "[^0-9]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        short terminocred1;
                        if (!string.IsNullOrEmpty(terminocred1Numerico))
                        {
                            terminocred1 = Convert.ToInt16(terminocred2Numerico);
                        }
                        else
                        {
                            terminocred1 = 0;
                        }



                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var docfiscalNumerico = Regex.Replace(valores[19], "[^0-9]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        int docfiscal;
                        if (!string.IsNullOrEmpty(docfiscalNumerico))
                        {
                            docfiscal = Convert.ToInt32(docfiscalNumerico);
                        }
                        else
                        {
                            docfiscal = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var terminocredXNumerico = Regex.Replace(valores[21], "[^0-9]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        short terminocredX;
                        if (!string.IsNullOrEmpty(terminocredXNumerico))
                        {
                            terminocredX = Convert.ToInt16(terminocredXNumerico);
                        }
                        else
                        {
                            terminocredX = 0;
                        }

                        var terminocredNumerico = Regex.Replace(valores[22], "[^0-9]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        short terminocred;
                        if (!string.IsNullOrEmpty(terminocredNumerico))
                        {
                            terminocred = Convert.ToInt16(terminocredNumerico);
                        }
                        else
                        {
                            terminocred = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var mas360Numerico = Regex.Replace(valores[24], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double mas360;
                        if (!string.IsNullOrEmpty(mas360Numerico))
                        {
                            mas360 = Convert.ToDouble(mas360Numerico);
                        }
                        else
                        {
                            mas360 = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var de271a360Numerico = Regex.Replace(valores[25], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double de271a360;
                        if (!string.IsNullOrEmpty(de271a360Numerico))
                        {
                            de271a360 = Convert.ToDouble(de271a360Numerico);
                        }
                        else
                        {
                            de271a360 = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var de211a270Numerico = Regex.Replace(valores[26], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double de211a270;
                        if (!string.IsNullOrEmpty(de211a270Numerico))
                        {
                            de211a270 = Convert.ToDouble(de211a270Numerico);
                        }
                        else
                        {
                            de211a270 = 0;
                        }


                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var de151a210Numerico = Regex.Replace(valores[27], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double de151a210;
                        if (!string.IsNullOrEmpty(de151a210Numerico))
                        {
                            de151a210 = Convert.ToDouble(de151a210Numerico);
                        }
                        else
                        {
                            de151a210 = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var de91a150Numerico = Regex.Replace(valores[28], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double de91a150;
                        if (!string.IsNullOrEmpty(de91a150Numerico))
                        {
                            de91a150 = Convert.ToDouble(de91a150Numerico);
                        }
                        else
                        {
                            de91a150 = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var de61a90Numerico = Regex.Replace(valores[29], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double de61a90;
                        if (!string.IsNullOrEmpty(de61a90Numerico))
                        {
                            de61a90 = Convert.ToDouble(de61a90Numerico);
                        }
                        else
                        {
                            de61a90 = 0;
                        }


                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var de31a60Numerico = Regex.Replace(valores[30], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double de31a60;
                        if (!string.IsNullOrEmpty(de31a60Numerico))
                        {
                            de31a60 = Convert.ToDouble(de31a60Numerico);
                        }
                        else
                        {
                            de31a60 = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var de16a30Numerico = Regex.Replace(valores[31], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double de16a30;
                        if (!string.IsNullOrEmpty(de16a30Numerico))
                        {
                            de16a30 = Convert.ToDouble(de16a30Numerico);
                        }
                        else
                        {
                            de16a30 = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var de1a15Numerico = Regex.Replace(valores[32], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double de1a15;
                        if (!string.IsNullOrEmpty(de1a15Numerico))
                        {
                            de1a15 = Convert.ToDouble(de1a15Numerico);
                        }
                        else
                        {
                            de1a15 = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var porvencerNumerico = Regex.Replace(valores[33], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double porvencer;
                        if (!string.IsNullOrEmpty(porvencerNumerico))
                        {
                            porvencer = Convert.ToDouble(porvencerNumerico);
                        }
                        else
                        {
                            porvencer = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var totalvencidoNumerico = Regex.Replace(valores[34], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double totalvencido;
                        if (!string.IsNullOrEmpty(totalvencidoNumerico))
                        {
                            totalvencido = Convert.ToDouble(totalvencidoNumerico);
                        }
                        else
                        {
                            totalvencido = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var subtotalNumerico = Regex.Replace(valores[35], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double subtotal;
                        if (!string.IsNullOrEmpty(subtotalNumerico))
                        {
                            subtotal = Convert.ToDouble(subtotalNumerico);
                        }
                        else
                        {
                            subtotal = 0;
                        }

                        // Verificar si telefonoCel contiene algo más que números y asignar "" en su lugar
                        var totalNumerico = Regex.Replace(valores[36], "[^0-9.-]", "");

                        // Convertir telefonoCelNumerico a un entero de manera segura
                        double total;
                        if (!string.IsNullOrEmpty(totalNumerico))
                        {
                            total = Convert.ToDouble(totalNumerico);
                        }
                        else
                        {
                            total = 0;
                        }

                        var modelo = new mdlCartera_Vencida
                        {
                            id = -999,
                            cliente = cliente, // Conversión a int
                            fecha = fechaCorregida,
                            telefono = valores[2],
                            telefonoCel = telefonoCel, 
                            sucursal = sucursal,
                            nombreSucursal = valores[5],
                            nombre = valores[6],
                            valororiginal = valororigen,
                            reg = reg,
                            pagado = pagado,
                            saldo = saldo,
                            credito = credito,
                            terminocred2 = terminocred2,
                            terminocred1 = terminocred1,
                            tipoclave = valores[14],
                            invo = valores[15],
                            origen = valores[16],
                            nombremodulo = valores[17],
                            seriefiscal = valores[18],
                            docfiscal = docfiscal,
                            nombremodulo2 = valores[20],
                            terminocredX = terminocredX,
                            terminocred = terminocred,
                            vencimiento = CorregirFecha(valores[23]),
                            mas360 = mas360,
                            de271a360 = de271a360,
                            de211a270 = de211a270,
                            de151a210 = de151a210,
                            de91a150 = de91a150,
                            de61a90 = de61a90,
                            de31a60 = de31a60,
                            de16a30 = de16a30,
                            de1a15 = de1a15,
                            porvencer = porvencer,
                            totalvencido = totalvencido,
                            subtotal = subtotal,
                            total = total,
                            usuario = "9013"
                        };
                        var result = await datos.Guardar(modelo);
                    }
                }

                return Ok(new { mensaje = "Datos cargados con éxito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al procesar el archivo: " + ex.Message });
            }
        }

    }
}
