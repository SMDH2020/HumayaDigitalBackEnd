using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace HDSecurity
{
    public class JwtValidateResult
    {
        public SecurityToken secutiryToken { get; private set; }
        public ClaimsPrincipal claimsPrincipal{ get; private set; }
        public JwtValidateResult(SecurityToken _securityToken, ClaimsPrincipal _claimsPrincipal)
        {
            this.secutiryToken = @_securityToken;
            this.claimsPrincipal = @_claimsPrincipal;
        }
    }
}
