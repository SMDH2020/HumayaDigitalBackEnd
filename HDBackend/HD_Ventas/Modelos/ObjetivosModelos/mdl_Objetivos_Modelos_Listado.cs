namespace HD_Ventas.Modelos.ObjetivosModelos
{
    public class mdl_Objetivos_Modelos_Listado
    {
        public int idgeneral { get; set; }
        public string? modelo { get; set; } = "";
        public int ejercicio { get; set; }
        public int idsucursal { get; set; }
        public int periodo { get; set; }
        public decimal? objetivo { get; set; }
    }
}
