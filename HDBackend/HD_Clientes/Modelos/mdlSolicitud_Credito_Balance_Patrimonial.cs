using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Balance_Patrimonial
    {

        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[SC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string? folio { get; set; }

        public double ac_cajabancos {get;set;}
        public double ac_clientes {get;set;}
        public double ac_deudoresdiversos {get;set;}
        public double ac_ivaporrecuperar {get;set;}
        public double ac_apoyodegobierno {get;set;}
        public double ac_inventariodeinsumos {get;set;}
        public double ac_inversionencultivos {get;set;}
        public double ac_otrosactivos {get;set;}
        public double af_terrenosenpropiedad {get;set;}
        public double af_terrenosenejidal {get;set;}
        public double af_construcciones {get;set;}
        public double af_maquinariayequipo {get;set;}
        public double af_equipodetransporte {get;set;}
        public double af_mobiliarioyequipo {get;set;}
        public double af_depresiaciones {get;set;}
        public double af_otrosactivos { get;set;}
        public double pc_creditosdirectos {get;set;}
        public double pc_creditosdeavio {get;set;}
        public double pc_proveedores {get;set;}
        public double pc_acreedoresdiversos {get;set;}
        public double pc_impuestosycuotas {get;set;}
        public double pc_amortizaciones {get;set;}
        public double pc_otrospasivos {get;set;}
        public double pf_creditosrefaccionarios {get;set;}
        public double pf_creditosdejdfm { get; set; }
        public double pf_otros {get;set;}
        public string? usuario { get; set; }
    }
}
