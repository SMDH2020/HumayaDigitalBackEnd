﻿using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlClientes_Datos_Contacto
    {
        [Required(ErrorMessage = "El idcliente es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo idcliente debe estar formado solo por numeros")]
        public int idcliente { get; set; }

        [Required(ErrorMessage = "El Orden es un valor requerido")]
        [RegularExpression(@"^[0-9]|-1$", ErrorMessage = "El campo Orden debe estar formado solo por numeros")]
        public int orden { get; set; }

        [Required(ErrorMessage = "El Medio de Contacto es un valor requerido")]
        [RegularExpression(@"^[TWC]+$", ErrorMessage = "El campo Medio de Contacto debe estar formado  por las siguientes opciones [T][W][C]")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo Medio de Contacto debe estar formado por 1 digitos")]
        public string? medio_contacto { get; set; } = "";

        [Required(ErrorMessage = "El Tipo de Contacto es un valor requerido")]
        [RegularExpression(@"^[COVE]+$", ErrorMessage = "El campo Tipo de Contacto debe estar formado  por las siguientes opciones [CO][VE]")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El campo Medio de Contacto debe estar formado por 2 digitos")]
        public string? tipo_contacto { get; set; }="";

        [Required(ErrorMessage = "El Valor es un valor requerido")]
        [RegularExpression(@"^[a-zA-Z0-9@-_.]+$", ErrorMessage = "El campo Tipo de Contacto debe estar formado  por las siguientes opciones [CO][VE]")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "El campo Medio de Contacto debe estar formado por 1 digitos")]
        public string? valor { get; set; } = "";

        [Required(ErrorMessage = "El Tipo de comentarios es un valor requerido")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El campo Comentarios debe estar formado por valores alfanumericos")]
        [StringLength(500, ErrorMessage = "El campo Comentarios debe de contener una longitud maxima de 500 caracteres")]
        public string? comentarios { get; set; } = "";

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
