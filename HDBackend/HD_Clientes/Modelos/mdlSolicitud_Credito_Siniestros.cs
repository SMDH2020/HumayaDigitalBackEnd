using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Siniestros
    {

        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[SC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string folio { get; set; }


        [Required(ErrorMessage = "El Registro es un valor requerido")]
        [RegularExpression(@"^[0-9]|-1$", ErrorMessage = "El campo Registro debe estar formado solo por numeros")]
        public short registro { get; set; }


        [Required(ErrorMessage = "El Siniestro es un valor requerido")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "El campo Siniestro debe estar formado por letras")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El campo Siniestro admite como maximo 50 caracteres")]
        public string siniestro { get; set; }

        [Required(ErrorMessage = "El Pago Total es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El Pago Total debe estar formado por numeros")]
        public double ptotal { get; set; }

        [Required(ErrorMessage = "El Pago Parcial es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El Pago Parcial debe estar formado por numeros")]
        public double pparcial { get; set; }

        [Required(ErrorMessage = "El ciclo es un valor requerido")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El campo Terreno debe estar formado solo por letras")]
        [StringLength(20, ErrorMessage = "El campo Ciclo debe contener una longitud maxima de 20 digitos")]
        public string ciclo { get; set; }

        public bool indemnizacion { get; set; }

        [Required(ErrorMessage = "El Monto es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Monto debe estar formado por numeros")]
        public double monto { get; set; }

        public bool estatus { get; set; }


        public string? usuario { get; set; } = "";
    }
}
