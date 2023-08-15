using HDSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HDEndpoints.Security
{
    public class JwtCodigoAutenticacionAttribute: AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authorizationHeader = actionContext.Request.Headers.Authorization;
            if (authorizationHeader != null && authorizationHeader.Scheme == "Bearer")
            {
                var token = authorizationHeader.Parameter;
                try
                {
                    var principal = JwtManager.GetPrincipal(token);
                    if (principal == null)
                    {
                        actionContext.Response = NoAutoriado();
                    }
                    else
                    {
                        SetPrincipal(actionContext, principal.claimsPrincipal);
                    }
                }
                catch
                {
                    actionContext.Response = NoAutoriado();
                }
            }
            else
            {
                actionContext.Response = NoAutoriado();
            }
        }
        private void SetPrincipal(HttpActionContext actionContext, ClaimsPrincipal principal)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            actionContext.RequestContext.Principal = principal;
        }
        HttpResponseMessage NoAutoriado()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("Los datos proporcionados no son correctos"),
                ReasonPhrase = "Unauthorized"
            };
            return response;
        }
    }
}