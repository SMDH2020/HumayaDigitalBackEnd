using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Cultivos
    {
        [Required(ErrorMessage = "El idsolicitudcultivo es un valor requerido")]
        [RegularExpression(@"^[0-9]|-1$", ErrorMessage = "El campo idsolicitudcultivo debe estar formado solo por numeros")]
        public int idsolicitud_cultivo {  get; set; }

        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^(SC[0-9]+|-999)$", ErrorMessage = "El campo folio debe estar formado por 'SC' seguido de números o ser '-999'")]
        public string folio { get; set; }

        [Required(ErrorMessage = "El idcultivo es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo idcultivo debe estar formado  por numeros")]
        public int idcultivo { get; set; }

        [Required(ErrorMessage = "Las Hectareas es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Hectareas debe estar formado  por numeros")]
        public double hectareas { get; set; }

        [Required(ErrorMessage = "El Ciclo es un valor requerido")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "El campo Ciclo debe estar formado por letras y numeros")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "El campo ciclo admite como maximo 20 caracteres")]
        public string ciclo { get; set; }

        [Required(ErrorMessage = "El Tipo de Riego es un valor requerido")]
        [RegularExpression(@"^[BGR]+$", ErrorMessage = "El campo Tipo de Riego debe estar formado por las siguientes opciones [B][G][R]")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo Tipo de Riego debe estar formado por 1 digitos")]
        public string tipo_riego { get; set; }

        [Required(ErrorMessage = "El Temporal es un valor requerido")]
        [RegularExpression(@"^[SN]+$", ErrorMessage = "El campo Temporal debe estar formado por las siguientes opciones [S][N]")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo Terreno debe estar formado por 1 digitos")]
        public string temporal { get; set; }


        [Required(ErrorMessage = "El Rendimiento es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Rendimiento debe estar formado solo por numeros")]
        public double rendimiento { get; set; }


        [Required(ErrorMessage = "El Precio es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Precio debe estar formado solo por numeros")]
        public double precio { get; set; }


        [Required(ErrorMessage = "El mes de cosecha es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Mes de Cosecha debe estar formado solo por numeros")]
        public int mes_cosecha { get; set; }

        public bool estatus { get; set; }


        [Required(ErrorMessage = "El Total de Toneladas es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Total Toneladas debe estar formado  por numeros")]
        public int total_toneladas { get; set; }


        public string? usuario { get; set; } = "";
    }
}
