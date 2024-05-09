using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Buro.Modelos
{
    public class mdlCartera_Vencida
    {
            public int id { get; set; }
            public long cliente { get; set; }
            public DateTime fecha { get; set; }
            public string? telefono { get; set; }
            public long telefonoCel { get; set; }
            public short sucursal { get; set; }
            public string? nombreSucursal { get; set; }
            public string? nombre { get; set; }
            public double valororiginal { get; set; }
            public int reg { get; set; }
            public double pagado { get; set; }
            public double saldo { get; set; }
            public double credito { get; set; }
            public short terminocred2 { get; set; }
            public short terminocred1 { get; set; }
            public string? tipoclave { get; set; }
            public string? invo { get; set; }
            public string? origen { get; set; }
            public string? nombremodulo { get; set; }
            public string? seriefiscal { get; set; }
            public int docfiscal { get; set; }
            public string? nombremodulo2 { get; set; }
            public short? terminocredX { get; set; }
            public short? terminocred { get; set; }
            public DateTime vencimiento { get; set; }
            public double mas360 { get; set; }
            public double de271a360 { get; set; }
            public double de211a270 { get; set; }
            public double de151a210 { get; set; }
            public double de91a150 { get; set; }
            public double de61a90 { get; set; }
            public double de31a60 { get; set; }
            public double de16a30 { get; set; }
            public double  de1a15 { get; set; }
            public double porvencer { get; set; }
            public double totalvencido { get; set; }
            public double subtotal { get; set; }
            public double total { get; set; }
            public string? usuario { get; set; } = "";
    }
}
