using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.AccesoDatos.BalanceGeneral.Complementos
{
    public static class DocUtil
    {

        public static string MonthName(int periodo)
        {
            string result = "";
            switch (periodo.ToString())
            {
                case "1":
                    result = "ENERO";
                    break;
                case "2":
                    result = "FEBRERO";
                    break;
                case "3":
                    result = "MARZO";
                    break;
                case "4":
                    result = "ABRIL";
                    break;
                case "5":
                    result = "MAYO";
                    break;
                case "6":
                    result = "JUNIO";
                    break;
                case "7":
                    result = "JULIO";
                    break;
                case "8":
                    result = "AGOSTO";
                    break;
                case "9":
                    result = "SEPTIEMBRE";
                    break;
                case "10":
                    result = "OCTUBRE";
                    break;
                case "11":
                    result = "NOVIEMBRE";
                    break;
                case "12":
                    result = "DICIEMBRE";
                    break;
            }
            return result;
        }
        public static string MonthName(string periodo)
        {
            var mes = periodo.Split(',');
            var result = "";
            if (mes.Length > 0)
            {
                var number = mes[mes.Length - 1];

                switch (number)
                {
                    case "1":
                        result = "ENERO";
                        break;
                    case "2":
                        result = "FEBRERO";
                        break;
                    case "3":
                        result = "MARZO";
                        break;
                    case "4":
                        result = "ABRIL";
                        break;
                    case "5":
                        result = "MAYO";
                        break;
                    case "6":
                        result = "JUNIO";
                        break;
                    case "7":
                        result = "JULIO";
                        break;
                    case "8":
                        result = "AGOSTO";
                        break;
                    case "9":
                        result = "SEPTIEMBRE";
                        break;
                    case "10":
                        result = "OCTUBRE";
                        break;
                    case "11":
                        result = "NOVIEMBRE";
                        break;
                    case "12":
                        result = "DICIEMBRE";
                        break;
                }

                return mes.Length > 1 ? "A " + result : result;
            }
            else
            {
                return "";
            }

        }

        public static string Rolado(int ejercicio, int mes)
        {
            int ejercicioinicial = mes < 12 ? ejercicio - 1 : ejercicio;
            int ejerciciofinal = ejercicio;
            int diff = 12 - (11 - mes);
            int mesinicio = mes == 12 ? 1 : diff;
            int mesfinal = mes;

            if (ejercicioinicial == ejerciciofinal)
                return $"{MonthName(mesinicio.ToString())} A {MonthName(mesfinal.ToString())} {ejerciciofinal}   (R12)";

            return $"{MonthName(mesinicio.ToString())} {ejercicioinicial} A {MonthName(mesfinal.ToString())} {ejerciciofinal} (R12)";


        }
    }
}
