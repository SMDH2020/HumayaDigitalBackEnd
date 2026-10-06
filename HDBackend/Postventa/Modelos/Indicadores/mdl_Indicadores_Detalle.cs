using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.Indicadores
{
    public class mdl_Indicadores_Detalle
    {
            public string Cliente { get; set; }
            public string Telefono { get; set; }
            public string Seccion { get; set; }
            public string Folio { get; set; }
            public int? idresponsable { get; set; }
            public string Responsable { get; set; }
            public int? IDSucursal { get; set; }
            public string Sucursal { get; set; }
            public DateTime? FechaMensaje { get; set; }
            public string Estatus { get; set; }
            public bool? ConRespuesta { get; set; }
            public bool? Facturado { get; set; }
            public DateTime? FechaFactura { get; set; }
            public decimal MontoFacturado { get; set; }

            // Totales del responsable (se repiten en cada renglon suyo)
            public int FacturadosTotal { get; set; }
            public int FacturadosMensajeria { get; set; }
            public decimal MontoTotal { get; set; }
            public decimal MontoMensajeria { get; set; }
            public decimal? PctMensajeria { get; set; }
        
    }
}
