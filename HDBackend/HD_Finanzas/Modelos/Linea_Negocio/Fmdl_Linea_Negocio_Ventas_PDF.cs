using HD_Finanzas.Modelos.Estado_Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.Linea_Negocio
{


    public class Fmdl_Linea_Negocio_Ventas_PDF
    {
        public string? subtitulo { get; set; }
        public Fmdl_Linea_negocio_Esquema_financiero data { get; set; }
        public List<Fmdl_Linea_Negocio_Ventas_Region> region { get; set; }
        public List<Fmdl_Linea_Negocio_Ventas_Sucursal> sucursal { get; set; }

    }


}
