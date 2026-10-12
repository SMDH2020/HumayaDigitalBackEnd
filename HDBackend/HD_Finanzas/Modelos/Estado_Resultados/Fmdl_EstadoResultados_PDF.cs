using HD_Finanzas.Modelos.Linea_Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.Estado_Resultados
{
    public class Fmdl_EstadoResultados_PDF
    {
        public string? subtitulo { get; set; }
        public string? periodoactual { get; set; }
        public string? periodoanterior { get; set; }
        public List<Fmdl_EstadoResultados_Data> data { get; set; }
        public List<Fmdl_EstadoResultados_Region> region { get; set; }
        public List<Fmdl_EstadoResultados_Sucursal> sucursal { get; set; }
    }
}
