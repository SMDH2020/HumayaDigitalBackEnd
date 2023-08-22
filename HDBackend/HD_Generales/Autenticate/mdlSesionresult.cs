namespace HD.Generales.Autenticate
{
    public class mdlSesionResult
    {
        public mdlSesion? sesion { get; set; } = new mdlSesion();
        public mdlSesionCodigoAutenticacion? autenticacion { get; set; } = new mdlSesionCodigoAutenticacion();
    }
}
