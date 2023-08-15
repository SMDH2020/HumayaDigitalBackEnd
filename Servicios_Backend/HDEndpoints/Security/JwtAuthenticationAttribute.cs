using HDSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HDEndpoints.Security
{
    public class JwtAuthenticationAttribute : AuthorizationFilterAttribute
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
                        List<Claim> claims = principal.claimsPrincipal.Claims.ToList();
                        if (claims.Count > 0)
                        {
                            Claim claim = claims[0];
                            if(claim.Type == ClaimTypes.Name)
                            {
                                string logger = claim.Value;
                                if (logger.Contains("~"))
                                {
                                    string[] loggincontiner = logger.Split('~');
                                    string usuario = loggincontiner[0];
                                    bool autenticado = bool.Parse(loggincontiner[1]);
                                    if(autenticado)
                                    {
                                        SetPrincipal(actionContext, principal.claimsPrincipal);
                                    }
                                    else
                                    {
                                        actionContext.Response = NoAutoriado();
                                    }
                                }
                                else
                                {
                                    actionContext.Response = NoAutoriado();
                                }
                            }
                            else
                            {
                                actionContext.Response = NoAutoriado();
                            }
                        }
                        else
                        {
                            actionContext.Response = NoAutoriado();
                        }
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