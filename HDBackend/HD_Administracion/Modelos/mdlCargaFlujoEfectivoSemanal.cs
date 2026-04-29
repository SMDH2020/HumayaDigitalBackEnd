namespace HD_Administracion.Modelos
{
    public class mdlCargaFlujoEfectivoSemanal
    {
        public int ejercicio {  get; set; }
        public int periodo { get; set; }
        public int id { get; set; }
        public string? nombre_periodo { get; set; }
        public int numero_semanas { get; set; }
        public string? fechainicio { get; set; }
        public string? fechafin {  get; set; }
        public string? encabezado1 { get; set; }
        public string? encabezado2 { get; set; }
        public string? encabezado3 { get; set; }
        public string? encabezado4 { get; set; }
        public string? encabezado5 { get; set; }
        public string? encabezadototal { get; set; }
        public int idconcepto { get; set; }
        public float proyeccion1 {  get; set; }
        public float real1 { get; set; }
        public float variacion1 { get; set; }
        public float porcentaje1 { get; set; }
        public float proyeccion2 { get; set; }
        public float real2 { get; set; }
        public float variacion2 { get; set; }
        public float porcentaje2 { get; set; }
        public float proyeccion3 { get; set; }
        public float real3 { get; set; }
        public float variacion3 { get; set; }
        public float porcentaje3 { get; set; }
        public float proyeccion4 { get; set; }
        public float real4 { get; set; }
        public float variacion4 { get; set; }
        public float porcentaje4 { get; set; }
        public float proyeccion5 { get; set; }
        public float real5 { get; set; }
        public float variacion5 { get; set; }
        public float porcentaje5 { get; set; }
        public float proyecciontotal { get; set; }
        public float realtotal { get; set; }
        public float variaciontotal { get; set; }
        public float porcentajetotal { get; set; }
        public string? concepto { get; set; }
        public string? tipo { get; set; }
    }
}
