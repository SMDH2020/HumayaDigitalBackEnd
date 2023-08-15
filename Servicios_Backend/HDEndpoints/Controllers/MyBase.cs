using HDEndpoints.Injection;
using HDEndpoints.Security;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dependencies;

namespace HDEndpoints.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs
    [JwtAuthentication]
    public class MyBase : ApiController
    {
        private readonly IDependencyResolver _dependencyResolver;
        public string CadenaConexion { get; private set; }
        public MyBase()
        {
            _dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            CadenaConexion = ConfigurationManager.ConnectionStrings["ConexionHDLogin"].ConnectionString;
        }

    
        public IUserSession UserSession => (IUserSession)_dependencyResolver.GetService(typeof(IUserSession));
    }
}