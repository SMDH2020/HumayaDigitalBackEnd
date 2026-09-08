namespace HD.Generales.Autenticate
{
    public class mdlUsuarios
    {
        public int idusuario { get; set; }
        public string? password { get; set; }
        public bool bloqueado { get; set; }
        public int intentos { get; set; }
        public int codigoautenticacion { get; set; }
    }
}
