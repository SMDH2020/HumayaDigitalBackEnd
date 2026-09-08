using HD.Generales.MCP.Config;
using HD.Generales.MCP.Services;
using Microsoft.AspNetCore.Http;

namespace HD.Generales.MCP.Security
{
    public class McpAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public McpAuthenticationMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, McpDataService data, McpJwtService jwt, McpAuthConfig config)
        {
            // Solo protege /mcp (el servidor de tools)
            // Excluye: /mcpauth/ (flujo de login) y /mcp/.well-known/ (discovery público)
            var path = context.Request.Path;
            bool esMcpProtegido = path.StartsWithSegments("/mcp")
                                  && !path.StartsWithSegments("/mcpauth")
                                  && !path.StartsWithSegments("/mcp/.well-known");

            if (esMcpProtegido)
            {
                var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    context.Response.Headers.WWWAuthenticate =
                        $"Bearer resource_metadata=\"{GetBaseUrl(context, config)}/mcp/.well-known/oauth-protected-resource\"";
                    await context.Response.WriteAsync("{\"error\":\"unauthorized\"}");
                    return;
                }

                var token = authHeader["Bearer ".Length..].Trim();
                var hash = jwt.HashToken(token);
                var tokenInfo = data.ValidarToken(hash);

                if (tokenInfo == null)
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    context.Response.Headers.WWWAuthenticate =
                        "Bearer error=\"invalid_token\"";
                    await context.Response.WriteAsync("{\"error\":\"invalid_token\"}");
                    return;
                }

                context.Items["McpUsuarioId"] = tokenInfo.McpUsuarioId;
                context.Items["McpEmail"] = tokenInfo.Email;
                context.Items["McpNombre"] = tokenInfo.Nombre;
            }

            await _next(context);
        }

        private static string GetBaseUrl(HttpContext context, McpAuthConfig config)
        {
            // Prioridad: 1) McpAuth:BaseUrl en appsettings  2) X-Forwarded headers  3) Request.Host
            if (!string.IsNullOrWhiteSpace(config?.BaseUrl))
                return config.BaseUrl.TrimEnd('/');

            var request = context.Request;
            var scheme = request.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? request.Scheme;
            var host   = request.Headers["X-Forwarded-Host"].FirstOrDefault()  ?? request.Host.Value;
            return $"{scheme}://{host}";
        }
    }
}
