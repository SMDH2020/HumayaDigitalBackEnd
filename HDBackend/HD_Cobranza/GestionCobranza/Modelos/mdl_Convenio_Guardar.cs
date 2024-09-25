namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Convenio_Guardar
    {
        public string? folio { get; set; }
        public int idcliente { get; set; }
        public string? razon_social { get; set; }
        public string? tipo_credito { get; set; }
        public string? referencia { get; set; }
        public int ADR { get; set; }
        public double monto { get; set; }
        public DateTime fecha_convenio { get; set; }
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
        public string? documento { get; set; } = "";
        public string? extension { get; set; } = "";
        public string? usuario { get; set; }
    }
}
