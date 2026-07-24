using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.DashboardRefacciones
{
    public class mdl_Dashboard_Refacciones
    {
        public int ejercicio { get; set; }
        public int periodo { get; set; }
        public int id { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? fecha { get; set; }
        public int idcliente { get; set; }
        public string? razon_social { get; set; }
        public double precio_lista { get; set; }
        public string? condicion { get; set; }
        public string? factura { get; set; }
        public int numero_folio { get; set; }
        public string? descripcion_tipo { get; set; }
        public int origen { get; set; }
        public string? vendedor { get; set; }
        public double descuento { get; set; }
        public double impuesto { get; set; }
        public string? folio { get; set; }
        public string? nota { get; set; }
        public double importe { get; set; }
        public double costo { get; set; }
        public string? tipo_credito { get; set; }
        public string? descripcion_tipo_credito { get; set; }
        public double ventas { get; set; }
        public double devoluciones { get; set; }
        public double salidas { get; set; }
        public double centradas { get; set; }
        public double total { get; set; }
        public double credito { get; set; }
        public double contado { get; set; }
        public string? parte { get; set; }
        public string? nombre_parte { get; set; }
        public string? familia { get; set; }
        public string? linea { get; set; }
        public string? subfamilia1 { get; set; }
        public string? subfamilia2 { get; set; }
        public double precio_unitario { get; set; }
        public double cantidad { get; set; }


    }
}
