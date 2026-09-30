namespace HD.Generales.Autenticate
{
    public class mdl_Linea_Venta_Usuario_Permiso
    {
        public int idrel { get; set; }
        public int idusuario { get; set; }
        public int idlinea { get; set; }
        public bool estatus { get; set; }
        public string? usuario { get; set; }
    }
}
