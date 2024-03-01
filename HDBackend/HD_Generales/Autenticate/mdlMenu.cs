namespace HD.Generales.Autenticate
{
    public class mdlMenu
    {
        public int idmenu { get; set; }
        public int idmodulo { get; set; }
        public string modulo { get; set; } = "";
        public string descripcion { get; set; } = "";
        public string nomenclatura { get; set; } = "";
        public bool estatus { get; set; }
        public string? usuario { get; set; } = "";
    }
}
