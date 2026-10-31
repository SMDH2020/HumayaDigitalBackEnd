namespace HD.Fiscal.Modelos
{
    public class mdl_Conciliacion_Aplicar
    {
        public int ejercicio { get; set; }
        public int periodo { get; set; }
        public string? detalle { get; set; }
        public string? comentario { get; set; }
        public int usuario { get; set; }
    }
}
