namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Gestion_Cobranza_Comentarios
    {
        public string? folio { get; set; }
        public int idcliente { get; set; }
        public string? razon_social { get; set; }
        public string? tipo_credito { get; set; }
        public string? referencia { get; set; }
        public int ADR { get; set; }
        public double monto { get; set; }
        public DateTime? fecha_convenio { get; set; }
        public bool recordatorio { get; set; }
        public DateTime? fecha_recordatorio { get; set; }
        public string? mediocontacto { get; set; }
        public string? firma { get; set; }
        public string? nombre_usuario { get; set; }
        public int idresponsable { get; set; }
        public double descuento { get; set; }
        public string? razon_descuento { get; set; }
        public string? detalle { get; set; } = "";
        public DateTime fecha_creacion { get; set; }
        public string? usuario { get; set; }
        public string? gestion { get; set; }
        public string? comentarios { get; set; }
        public bool volvercontactar { get; set; }
        public DateTime? fechavolveracontactar { get; set; }
        public float saldo { get; set; }
        public float moratorios { get; set; }
        public float interespactado { get; set; }
        public float total { get; set; }
    }
}
