namespace HD.Generales.Autenticate
{
    public class mdlSesionresult
    {
         public mdlSesion? sesion { get; set; }=new mdlSesion();
        public mdlSesionCodigoAutenticacion? autenticacion { get; set; }= new mdlSesionCodigoAutenticacion();
    }
}