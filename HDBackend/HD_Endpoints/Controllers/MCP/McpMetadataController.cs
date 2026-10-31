using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.MCP
{
    [Route("mcp/.well-known")]
    [Route("mcpauth/.well-known")]
    [ApiController]
    public class McpMetadataController : ControllerBase
    {
        [HttpGet("oauth-authorization-server")]
        public IActionResult Metadata()
        {
            var base_url = "https://humayadigital.com/mh/apihumayadigital";
            //var base_url = "https://today-gulf-bowl-incident.trycloudflare.com";

            return Ok(new
            {
                issuer = $"{base_url}/mcp",
                authorization_endpoint = $"{base_url}/mcpauth/oauth/login_authorize",
                token_endpoint = $"{base_url}/mcpauth/oauth/token",
                revocation_endpoint = $"{base_url}/mcpauth/oauth/revoke",
                response_types_supported = new[] { "code" },
                registration_endpoint = $"{base_url}/mcpauth/oauth/register",
                grant_types_supported = new[] { "authorization_code" },
                token_endpoint_auth_methods_supported = new[] { "none" }
            });
        }
    }
}
