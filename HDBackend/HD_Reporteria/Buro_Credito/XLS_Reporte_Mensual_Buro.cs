using ClosedXML.Excel;
using HD.AccesoDatos;
using HD_Buro.Modelos;
using HD_Cobranza;
using HD_Cobranza.Reportes;

namespace HD_Reporteria.Buro_Credito
{
    public class XLS_Reporte_Mensual_Buro
    {
        public static Task<DocResult> CrearExcel(IEnumerable<mdlInformeBuroCredito> lista,string periodo,int ejercicio)
        {
            try
            {
                string sheetname = "Buro de Credito " + periodo + " " + ejercicio.ToString();
                sheetname=sheetname.ToUpper();  
                string ruta = $"C:\\SMDH\\Procesados\\{sheetname}.xlsx";
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add(sheetname);
                    sheet.Style.Font.FontName = "Calibri";
                    sheet.Style.Font.FontSize = 10;

                    int renglon = XLSEncabezado.Encabezado(ref sheet, sheetname, 111);
                    sheet.Cell(renglon, 1).Value = "Identificador";
                    sheet.Cell(renglon, 2).Value = "RFC";
                    sheet.Cell(renglon, 3).Value = "Codigo Ciudadano";
                    sheet.Cell(renglon, 4).Value = "RAZON Numero Dun";
                    sheet.Cell(renglon, 5).Value = "Compañia";
                    sheet.Cell(renglon, 6).Value = "Nombre 1";
                    sheet.Cell(renglon, 7).Value = "Nombre 2";
                    sheet.Cell(renglon, 8).Value = "Paterno";
                    sheet.Cell(renglon, 9).Value = "Materno";
                    sheet.Cell(renglon, 10).Value = "Nacionalidad";
                    sheet.Cell(renglon, 11).Value = "Calificacion Banco de Mex.";
                    sheet.Cell(renglon, 12).Value = "Banxico 1";
                    sheet.Cell(renglon, 13).Value = "Banxico 2";
                    sheet.Cell(renglon, 14).Value = "Banxico 3";
                    sheet.Cell(renglon, 15).Value = "Direccion 1";
                    sheet.Cell(renglon, 16).Value = "Direccion 2";
                    sheet.Cell(renglon, 17).Value = "Colonia/Poblacion";
                    sheet.Cell(renglon, 18).Value = "Delegacion/Municipio";
                    sheet.Cell(renglon, 19).Value = "Ciudad";
                    sheet.Cell(renglon, 20).Value = "Estado";
                    sheet.Cell(renglon, 21).Value = "C.P.";
                    sheet.Cell(renglon, 22).Value = "Telefono";
                    sheet.Cell(renglon, 23).Value = "Extension";
                    sheet.Cell(renglon, 24).Value = "Fax";
                    sheet.Cell(renglon, 25).Value = "Tipo Cliente";
                    sheet.Cell(renglon, 26).Value = "Estado extranjero";
                    sheet.Cell(renglon, 27).Value = "Pais";
                    sheet.Cell(renglon, 28).Value = "Clave de Consolidacion";
                    sheet.Cell(renglon, 29).Value = "Identificador";
                    sheet.Cell(renglon, 30).Value = "RFC Accionista";
                    sheet.Cell(renglon, 31).Value = "Codigo Ciudadano";
                    sheet.Cell(renglon, 32).Value = "Numero Dun";
                    sheet.Cell(renglon, 33).Value = "Nombre Cia";
                    sheet.Cell(renglon, 34).Value = "Nombre 1";
                    sheet.Cell(renglon, 35).Value = "Nombre 2";
                    sheet.Cell(renglon, 36).Value = "Paterno";
                    sheet.Cell(renglon, 37).Value = "Materno";
                    sheet.Cell(renglon, 38).Value = "Porcentaje";
                    sheet.Cell(renglon, 39).Value = "Direccion 1";
                    sheet.Cell(renglon, 40).Value = "Direccion 2";
                    sheet.Cell(renglon, 41).Value = "Colonia/Poblacion";
                    sheet.Cell(renglon, 42).Value = "Delegacion/Municipio";
                    sheet.Cell(renglon, 43).Value = "Ciudad";
                    sheet.Cell(renglon, 44).Value = "Estado";
                    sheet.Cell(renglon, 45).Value = "C.P.";
                    sheet.Cell(renglon, 46).Value = "Telefono";
                    sheet.Cell(renglon, 47).Value = "Extension";
                    sheet.Cell(renglon, 48).Value = "Fax";
                    sheet.Cell(renglon, 49).Value = "Tipo Cliente";
                    sheet.Cell(renglon, 50).Value = "Extado extranjero";
                    sheet.Cell(renglon, 51).Value = "Pais";
                    sheet.Cell(renglon, 52).Value = "Identificador";
                    sheet.Cell(renglon, 53).Value = "RFC Empresa";
                    sheet.Cell(renglon, 54).Value = "Numero Experiencias";
                    sheet.Cell(renglon, 55).Value = "Contrado";
                    sheet.Cell(renglon, 56).Value = "Contrado Anterior";
                    sheet.Cell(renglon, 57).Value = "Fecha Apertura";
                    sheet.Cell(renglon, 58).Value = "Plazo en meses";
                    sheet.Cell(renglon, 59).Value = "Tipo de Credito";
                    sheet.Cell(renglon, 60).Value = "Saldo Inicial";
                    sheet.Cell(renglon, 61).Value = "Moneda";
                    sheet.Cell(renglon, 62).Value = "Numero Pagos";
                    sheet.Cell(renglon, 63).Value = "Frecuencia de Pagos";
                    sheet.Cell(renglon, 64).Value = "Importe de pagos";
                    sheet.Cell(renglon, 65).Value = "Fecha ultimo pago";
                    sheet.Cell(renglon, 66).Value = "Fecha Reestructura";
                    sheet.Cell(renglon, 67).Value = "Pago en efectivo";
                    sheet.Cell(renglon, 68).Value = "Fecha Liquidacion";
                    sheet.Cell(renglon, 69).Value = "Quita";
                    sheet.Cell(renglon, 70).Value = "Dacion";
                    sheet.Cell(renglon, 71).Value = "Quebranto";
                    sheet.Cell(renglon, 72).Value = "Observaciones";
                    sheet.Cell(renglon, 73).Value = "Especiales";
                    sheet.Cell(renglon, 74).Value = "Fecha Primer Incum";
                    sheet.Cell(renglon, 75).Value = "Saldo Insoluto";
                    sheet.Cell(renglon, 76).Value = "Credito Maximo Utilizado";
                    sheet.Cell(renglon, 77).Value = "Cartera vencida";
                    sheet.Cell(renglon, 78).Value = "Identificador";
                    sheet.Cell(renglon, 78).Value = "RFC Empresa";
                    sheet.Cell(renglon, 80).Value = "Contrato";
                    sheet.Cell(renglon, 81).Value = "Dias Vencimiento";
                    sheet.Cell(renglon, 82).Value = "Cantidad";
                    sheet.Cell(renglon, 83).Value = "Interes";
                    sheet.Cell(renglon, 84).Value = "Identificador";
                    sheet.Cell(renglon, 85).Value = "RFC Aval";
                    sheet.Cell(renglon, 86).Value = "Codigo Ciudadano";
                    sheet.Cell(renglon, 87).Value = "Numero Dun";
                    sheet.Cell(renglon, 88).Value = "Nombre Cia";
                    sheet.Cell(renglon, 89).Value = "Nombre 1";
                    sheet.Cell(renglon, 90).Value = "Nombre 2";
                    sheet.Cell(renglon, 91).Value = "Paterno";
                    sheet.Cell(renglon, 92).Value = "Materno";
                    sheet.Cell(renglon, 93).Value = "Direccion 1";
                    sheet.Cell(renglon, 94).Value = "Direccion 2";
                    sheet.Cell(renglon, 95).Value = "Colonia/Poblacion";
                    sheet.Cell(renglon, 96).Value = "Delegacion/Municipio";
                    sheet.Cell(renglon, 97).Value = "Ciudad";
                    sheet.Cell(renglon, 98).Value = "Estado";
                    sheet.Cell(renglon, 99).Value = "C.P.";
                    sheet.Cell(renglon, 100).Value = "Telefono";
                    sheet.Cell(renglon, 101).Value = "Extension";
                    sheet.Cell(renglon, 102).Value = "Fax";
                    sheet.Cell(renglon, 103).Value = "Tipo Cliente";
                    sheet.Cell(renglon, 104).Value = "Extado extranjero";
                    sheet.Cell(renglon, 105).Value = "Pais";
                    sheet.Cell(renglon, 106).Value = "Identificador";
                    sheet.Cell(renglon, 107).Value = "Numero de compañias";
                    sheet.Cell(renglon, 108).Value = "Cantidad";
                    sheet.Cell(renglon, 109).Value = "Identificador";
                    sheet.Cell(renglon, 110).Value = "Numero de Compañias";
                    sheet.Cell(renglon, 111).Value = "Cantidad";




                    var rango = sheet.Range(renglon, 1, renglon, 111);
                    rango.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBECEE");
                    rango.Style.Font.Bold = true;
                    rango.Style.Font.FontSize = 12;
                    rango.RangeUsed().SetAutoFilter();
                    rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    renglon++;


                    foreach (mdlInformeBuroCredito mdl in lista)
                    {
                        sheet.Cell(renglon, 1).Value = "EM"; // Identificador
                        sheet.Cell(renglon, 2).Value = mdl.rfc; // RFC
                        sheet.Cell(renglon, 3).Value = ""; // Codigo Ciudadano
                        sheet.Cell(renglon, 4).Value = ""; // RAZON Numero Dun
                        sheet.Cell(renglon, 5).Value = mdl.compania; // Compañia
                        sheet.Cell(renglon, 6).Value = mdl.nombre1; // Nombre 1
                        sheet.Cell(renglon, 7).Value = mdl.nombre2; // Nombre 2
                        sheet.Cell(renglon, 8).Value = mdl.paterno; // Paterno
                        sheet.Cell(renglon, 9).Value = mdl.materno; // Materno
                        sheet.Cell(renglon, 10).Value = "MX"; // Nacionalidad
                        sheet.Cell(renglon, 11).Value = mdl.banxico; // Calificacion Banco de Mex.
                        sheet.Cell(renglon, 12).Value = ""; // Banxico 1
                        sheet.Cell(renglon, 13).Value = ""; // Banxico 2
                        sheet.Cell(renglon, 14).Value = ""; // Banxico 3
                        sheet.Cell(renglon, 15).Value = mdl.direccion1; // Direccion 1
                        sheet.Cell(renglon, 16).Value = mdl.direccion2; // Direccion 2
                        sheet.Cell(renglon, 17).Value = mdl.poblacion; // Colonia/Poblacion
                        sheet.Cell(renglon, 18).Value = mdl.domicilio; // Delegacion/Municipio
                        sheet.Cell(renglon, 19).Value = mdl.ciudad; // Ciudad
                        sheet.Cell(renglon, 20).Value = mdl.estado; // Estado
                        sheet.Cell(renglon, 21).Value = mdl.cp; // C.P.
                        sheet.Cell(renglon, 22).Value = ""; // Telefono
                        sheet.Cell(renglon, 23).Value = ""; // Extension
                        sheet.Cell(renglon, 24).Value = ""; // Fax
                        sheet.Cell(renglon, 25).Value = mdl.tipocliente; // Tipo Cliente
                        sheet.Cell(renglon, 26).Value = ""; // Estado extranjero
                        sheet.Cell(renglon, 27).Value = "MX"; // Pais
                        sheet.Cell(renglon, 28).Value = ""; // Clave de Consolidacion
                        sheet.Cell(renglon, 29).Value = ""; // Identificador
                        sheet.Cell(renglon, 30).Value = ""; // RFC Accionista
                        sheet.Cell(renglon, 31).Value = ""; // Codigo Ciudadano
                        sheet.Cell(renglon, 32).Value = ""; // Numero Dun
                        sheet.Cell(renglon, 33).Value = ""; // Nombre Cia
                        sheet.Cell(renglon, 34).Value = ""; // Nombre 1
                        sheet.Cell(renglon, 35).Value = ""; // Nombre 2
                        sheet.Cell(renglon, 36).Value = ""; // Paterno
                        sheet.Cell(renglon, 37).Value = ""; // Materno
                        sheet.Cell(renglon, 38).Value = ""; // Porcentaje
                        sheet.Cell(renglon, 39).Value = ""; // Direccion 1
                        sheet.Cell(renglon, 40).Value = ""; // Direccion 2
                        sheet.Cell(renglon, 41).Value = ""; // Colonia/Poblacion
                        sheet.Cell(renglon, 42).Value = ""; // Delegacion/Municipio
                        sheet.Cell(renglon, 43).Value = ""; // Ciudad
                        sheet.Cell(renglon, 44).Value = ""; // Estado
                        sheet.Cell(renglon, 45).Value = ""; // C.P.
                        sheet.Cell(renglon, 46).Value = ""; // Telefono
                        sheet.Cell(renglon, 47).Value = ""; // Extension
                        sheet.Cell(renglon, 48).Value = ""; // Fax
                        sheet.Cell(renglon, 49).Value = ""; // Tipo Cliente
                        sheet.Cell(renglon, 50).Value = ""; // Extado extranjero
                        sheet.Cell(renglon, 51).Value = ""; // Pais
                        sheet.Cell(renglon, 52).Value = "CR"; // Identificador
                        sheet.Cell(renglon, 53).Value = mdl.rfc; // RFC Empresa
                        sheet.Cell(renglon, 54).Value = mdl.facturas; // Numero Experiencias
                        sheet.Cell(renglon, 55).Value = ""; // Contrado
                        sheet.Cell(renglon, 56).Value = ""; // Contrado Anterior
                        sheet.Cell(renglon, 57).Value = ""; // Fecha Apertura
                        sheet.Cell(renglon, 58).Value = ""; // Plazo en meses
                        sheet.Cell(renglon, 59).Value = ""; // Tipo de Credito
                        sheet.Cell(renglon, 60).Value = ""; // Saldo Inicial
                        sheet.Cell(renglon, 61).Value = "001"; // Moneda
                        sheet.Cell(renglon, 62).Value = ""; // Numero Pagos
                        sheet.Cell(renglon, 63).Value = ""; // Frecuencia de Pagos
                        sheet.Cell(renglon, 64).Value = ""; // Importe de pagos
                        sheet.Cell(renglon, 65).Value = ""; // Fecha ultimo pago
                        sheet.Cell(renglon, 66).Value = ""; // Fecha Reestructura
                        sheet.Cell(renglon, 67).Value = ""; // Pago en efectivo
                        sheet.Cell(renglon, 68).Value = ""; // Fecha Liquidacion
                        sheet.Cell(renglon, 69).Value = ""; // Quita
                        sheet.Cell(renglon, 70).Value = ""; // Dacion
                        sheet.Cell(renglon, 71).Value = ""; // Quebranto
                        sheet.Cell(renglon, 72).Value = ""; // Observaciones
                        sheet.Cell(renglon, 73).Value = ""; // Especiales
                        sheet.Cell(renglon, 74).Value = ""; // Fecha Primer Incum
                        sheet.Cell(renglon, 75).Value = ""; // Saldo Insoluto
                        sheet.Cell(renglon, 76).Value = ""; // Credito Maximo Utilizado
                        sheet.Cell(renglon, 77).Value = ""; // Cartera vencida
                        sheet.Cell(renglon, 78).Value = "DET"; // Identificador
                        sheet.Cell(renglon, 79).Value = mdl.rfc; // RFC Empresa
                        sheet.Cell(renglon, 80).Value = ""; // Contrato
                        sheet.Cell(renglon, 81).Value = mdl.diasvencido; // Dias Vencimiento
                        sheet.Cell(renglon, 82).Value = Math.Round(decimal.Parse(mdl.saldo.ToString()),0); // Cantidad
                        sheet.Cell(renglon, 83).Value = ""; // Interes
                        sheet.Cell(renglon, 84).Value = ""; // Identificador
                        sheet.Cell(renglon, 85).Value = ""; // RFC Aval
                        sheet.Cell(renglon, 86).Value = ""; // Codigo Ciudadano
                        sheet.Cell(renglon, 87).Value = ""; // Numero Dun
                        sheet.Cell(renglon, 88).Value = ""; // Nombre Cia
                        sheet.Cell(renglon, 89).Value = ""; // Nombre 1
                        sheet.Cell(renglon, 90).Value = ""; // Nombre 2
                        sheet.Cell(renglon, 91).Value = ""; // Paterno
                        sheet.Cell(renglon, 92).Value = ""; // Materno
                        sheet.Cell(renglon, 93).Value = ""; // Direccion 1
                        sheet.Cell(renglon, 94).Value = ""; // Direccion 2
                        sheet.Cell(renglon, 95).Value = ""; // Colonia/Poblacion
                        sheet.Cell(renglon, 96).Value = ""; // Delegacion/Municipio
                        sheet.Cell(renglon, 97).Value = ""; // Ciudad
                        sheet.Cell(renglon, 98).Value = ""; // Estado
                        sheet.Cell(renglon, 99).Value = ""; // C.P.
                        sheet.Cell(renglon, 100).Value = ""; // Telefono
                        sheet.Cell(renglon, 101).Value = ""; // Extension
                        sheet.Cell(renglon, 102).Value = ""; // Fax
                        sheet.Cell(renglon, 103).Value = ""; // Tipo Cliente
                        sheet.Cell(renglon, 104).Value = ""; // Extado extranjero
                        sheet.Cell(renglon, 105).Value = ""; // Pais
                        sheet.Cell(renglon, 106).Value = ""; // Identificador
                        sheet.Cell(renglon, 107).Value = ""; // Numero de compañias
                        sheet.Cell(renglon, 108).Value = ""; // Cantidad


                        renglon++;
                    }
                    renglon -= 1;
                    var group = lista.GroupBy(item => item.rfc);
                    var suma = lista.Sum(item => item.saldo);
                    sheet.Cell(renglon, 109).Value = "TS";//SEccion TS
                    sheet.Cell(renglon, 110).Value = group.Count(); ;//Numero de compañias
                    sheet.Cell(renglon, 111).Value = suma;//Cantidad


                    sheet.Columns().AdjustToContents();
                    workbook.SaveAs(ruta);

                }
                if (System.IO.File.Exists(ruta))
                {
                    byte[] docbytes = System.IO.File.ReadAllBytes(ruta);
                    string docBase64 = Convert.ToBase64String(docbytes);
                    System.IO.File.Delete(ruta);
                    DocResult doc = new DocResult
                    {
                        documento = docBase64,
                        filename = sheetname
                    };
                    return Task.FromResult(doc);
                }
                throw new Exception("ERROR EN LA GENERACION DEL ARCHIVO, FAVOR DE COMUNICARSE CON EL ADMINISTRADOR DEL SISTEMA");
            }
            catch (Exception ex)
            {
                throw new Excepciones(System.Net.HttpStatusCode.InternalServerError, new { errores = ex.Message });
            }
        }
    }
}
