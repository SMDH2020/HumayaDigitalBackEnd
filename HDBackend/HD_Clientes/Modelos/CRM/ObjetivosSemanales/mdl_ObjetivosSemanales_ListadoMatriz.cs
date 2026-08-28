namespace HD.Clientes.Modelos.CRM.ObjetivosSemanales
{
    /// <summary>
    /// Renglon de la matriz de objetivos por linea (configuracion global, sin vendedor).
    /// updateuser y updatedate son nullable: un registro recien insertado no los trae.
    /// </summary>
    public class mdl_ObjetivosSemanales_ListadoMatriz
    {
        public int idrelacion { get; set; }
        public int idlinea { get; set; }
        public string? linea { get; set; } = "";
        public decimal objetivo { get; set; }
        public bool estatus { get; set; }
        public int createuser { get; set; }
        public DateTime createdate { get; set; }
        public int? updateuser { get; set; }
        public DateTime? updatedate { get; set; }
    }
}
