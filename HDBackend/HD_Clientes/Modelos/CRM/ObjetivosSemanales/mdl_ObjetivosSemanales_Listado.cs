namespace HD.Clientes.Modelos.CRM.ObjetivosSemanales
{
    /// <summary>
    /// Renglon del listado de objetivos semanales por vendedor y linea.
    /// </summary>
    public class mdl_ObjetivosSemanales_Listado
    {
        public int idsemana { get; set; }
        public string? semana { get; set; } = "";
        public int idvendedor { get; set; }
        public string? vendedor { get; set; } = "";
        public int idlinea { get; set; }
        public string? linea { get; set; } = "";
        public int idestado { get; set; }
        public string? estado { get; set; } = "";
        public decimal objetivo { get; set; }
    }
}
