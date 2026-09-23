using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Modal
{
    public class mdl_Tabla_Documentos_Faltantes
    {
        public string Folio { get; set; }
        public int IdDocumento { get; set; }
        public string? Documento { get; set; }
        public bool Cargado { get; set; }

    }
}
