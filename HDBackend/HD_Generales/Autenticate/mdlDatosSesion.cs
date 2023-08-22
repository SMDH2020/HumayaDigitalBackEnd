namespace HD.Generales.Autenticate
{
    public class mdlDatosSesion
    {
        public mdlLoginResult usuario { get; set; } = new mdlLoginResult();
        public IEnumerable<mdlMenu> menus { get; set; } = new   List<mdlMenu>();    
        public IEnumerable<mdlModulo> modulos { get; set; } = new List<mdlModulo>();
    }
}
