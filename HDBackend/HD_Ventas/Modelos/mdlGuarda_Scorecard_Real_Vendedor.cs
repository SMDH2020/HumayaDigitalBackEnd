namespace HD_Ventas.Modelos
{
    public class mdlGuarda_Scorecard_Real_Vendedor
    {
        public int idproyeccion { get; set; }
        public int ejercicio { get; set; }
        public int periodo { get; set; }
        public int sucursal { get; set; }
        public int idvendedor { get; set; }
        public int idlinea { get; set; }
        public int objetivo { get; set; }
        public int unidades_vendidas { get; set; }
        public bool objetivo_habilitado { get; set; }
        public bool unidades_vendidas_habilitado { get; set; }
        public string? scorecard_detalle { get; set; } = "";
        public string? usuario { get; set; }
    }
}
