using System.Web;

namespace HDEndpoints.Injection
{
    public interface IUserSession
    {
        string GetUsuarioSessionID();
        string GetUsuarioSessionName();
    }

    public class UserSession : IUserSession
    {
        private readonly HttpContextBase _httpContext;
        public UserSession(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }
        public string GetUsuarioSessionID()
        {
            var result = _httpContext.User;
            return "";
        }

        public string GetUsuarioSessionName()
        {
            return "";
        }
    }

}