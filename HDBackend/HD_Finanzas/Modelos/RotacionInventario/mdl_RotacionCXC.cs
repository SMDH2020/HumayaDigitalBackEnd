using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.RotacionInventario
{
    public class mdl_RotacionCXC
    {
        public int iddepartamento { get; set; }
        public string? departamento { get; set; }
        public int idestado { get; set; }
        public string? estado { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public double saldo_inicial { get; set; }
        public double credito { get; set; }
        public double saldo_final { get; set; }
        public double rcxc { get; set; }
        public double guia { get; set; }
        public double guia_semestral { get; set; }
        public double guia_anual { get; set; }
        public double rcxc_semestral { get; set; }
        public double rcxc_anual { get; set; }
        public double cartera_optima { get; set; }
        public double diferencia_cartera_optima { get; set; }


    }
}
