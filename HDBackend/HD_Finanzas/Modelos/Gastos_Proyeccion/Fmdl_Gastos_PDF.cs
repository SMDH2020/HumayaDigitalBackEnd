using HD_Finanzas.Modelos.Estado_Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.Gastos_Proyeccion
{
    public class Fmdl_Gastos_PDF
    {
        public string? subtitulo { get; set; }
        public string? periodoactual { get; set; }
        public string? periodoanterior { get; set; }
        public List<Fmdl_GastosPorConcepto> data { get; set; }
        public List<Fmdl_Gastos_Region> region { get; set; }
        public List<Fmdl_Gastos_Sucursal> sucursal { get; set; }
        public List<Fmdl_Gastos_Departamento> departamento { get; set; }
    }
}
