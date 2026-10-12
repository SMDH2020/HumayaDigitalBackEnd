using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos
{
    public class prmLineas
    {
        public static string Value(string str)
        {
            string result = "";
            if (str.Length > 0)
            {
                string[] lineas = str.Split(',');
                for (int index = 0; index < lineas.Length; index++)
                {
                    result += (result.Length > 0 ? "," : "") + $"'{lineas[index]}'";
                }

            }
            return result;
        }
    }
}
