using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlClientes_Cultivo_Listado
    {

        [Required(ErrorMessage = "El idcliente es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo idcliente debe estar formado solo por numeros")]
        public int idcliente { get; set; }

        [Required(ErrorMessage = "El Registro es un valor requerido")]
        [RegularExpression(@"^-?[0-9]+$", ErrorMessage = "El campo registro debe estar formado solo por números, pudiendo ser negativo")]
        public int registro { get; set; }


        [Required(ErrorMessage = "El idcultivo es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo idcultivo debe estar formado solo por numeros")]
        public int idcultivo { get; set; }

        [Required(ErrorMessage = "El cultivo es un valor requerido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El campo Cultivo debe estar formado solo por letras")]
        public string? cultivo { get; set; } = "";

        [Required(ErrorMessage = "El Terreno es un valor requerido")]
        [RegularExpression(@"^[PRES]+$", ErrorMessage = "El campo Terreno debe estar formado solo por letras")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo Terreno debe estar formado por 1 digitos")]
        public string terreno { get; set; }


        [Required(ErrorMessage = "Las Hectareas es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Hectareas debe estar formado solo por numeros")]
        public double hectareas { get; set; }



        [Required(ErrorMessage = "El Seguro de Cosecha es un valor requerido")]
        [RegularExpression(@"^[SNC]+$", ErrorMessage = "El Seguro de Cosecha debe estar formado solo por letras")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo Seguro de Cosecha debe estar formado por 1 digitos")]
        public string seguro_cosecha { get; set; }

        [Required(ErrorMessage = "El ciclo es un valor requerido")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El campo Terreno debe estar formado solo por letras")]
        [StringLength(20, ErrorMessage = "El campo Ciclo debe contener una longitud maxima de 20 digitos")]
        public string ciclo { get; set; }

        [Required(ErrorMessage = "El Tipo de Riego es un valor requerido")]
        [RegularExpression(@"^[BGR]+$", ErrorMessage = "El campo Tipo de Riego debe estar formado solo por letras")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo Tipo de Riego debe estar formado por 1 digitos")]
        public string tipo_riego { get; set; }

        [Required(ErrorMessage = "El Temporal es un valor requerido")]
        [RegularExpression(@"^[SN]+$", ErrorMessage = "El campo Temporal debe estar formado solo por letras")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo Terreno debe estar formado por 1 digitos")]
        public string temporal { get; set; }

        [Required(ErrorMessage = "El Rendimiento es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Rendimiento debe estar formado solo por numeros")]
        public double rendimiento { get; set; }

        [Required(ErrorMessage = "El Precio es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Precio debe estar formado solo por numeros")]
        public double precio { get; set; }

        [Required(ErrorMessage = "El mes de cosecha es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Mes de Cosecha debe estar formado solo por letras")]
        public string mescosecha { get; set; }

        public bool estatus { get; set; } = true;
    }
}
