using HDEndpoints.Injection;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace HDEndpoints.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container
                .RegisterType<IUserSession, UserSession>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}