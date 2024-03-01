namespace HD.Generales.Autenticate
{
    public class mdlDatosSesion_Movil
    {
        public mdlLoginResult usuario { get; set; } = new mdlLoginResult();
        public IEnumerable<mdlPresas_Niveles>? presas { get; set; }
    }
}
