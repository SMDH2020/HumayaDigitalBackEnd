using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Garantias_CSV
    {
        public string NúmerosDeSerie { get; set; }
        public string Modelo { get; set; }
        public string Cuentas { get; set; }
        public string Concesionario { get; set; }
        public string CiudadDeConcesion { get; set; }
        public string NombreDeCliente { get; set; }
        public string Telefono { get; set; }
        public string Calle1 { get; set; }
        public string Calle2 { get; set; }
        public string CodigoPostal { get; set; }
        public string Ciudad { get; set; }
        public string Region { get; set; }
        public string Pais { get; set; }
        public string? InicioGarantia { get; set; }
        public string? Expiracion { get; set; }
        public string LimiteTiempo { get; set; }
        public string TipoGarantia { get; set; }
        public string TipoCobertura { get; set; }
        public string ContratoAdquirido { get; set; }
    }
}
