namespace HD.Generales.Modelos
{
    public class mdl_Presentaciones_Guardar
    {
        public Guid presentacionId { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string usuario { get; set; }
    }
    public class mdl_Presentaciones_Html
    {
        public Guid presentacionId { get; set; }
        public string  htmlContenido { get; set; }
        public string usuario { get; set; }
    }
    public class mld_Presentaciones_Listado
    {
        public Guid presentacionId { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string htmlContenido { get; set; }
        public string usuario { get; set; }
    }
}
