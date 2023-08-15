using System.Collections.Generic;

namespace HD_Generales.Autenticate
{
    public class mdlDatosSesion
    {
        public mdlALoginResult usuario { get; set; }
        public IEnumerable<mdlMenu> menus { get; set; }
        public IEnumerable<mdlModulos> modulos { get; set; }
    }
}
