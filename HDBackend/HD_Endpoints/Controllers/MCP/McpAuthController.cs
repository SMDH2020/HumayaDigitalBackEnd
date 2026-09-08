using HD.Generales.MCP.Config;
using HD.Generales.MCP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HD.Endpoints.Controllers.MCP
{
    [AllowAnonymous]
    [Route("mcpauth/oauth")]
    public class McpAuthController : Controller
    {
        private readonly McpDataService _data;
        private readonly McpEmailService _email;
        private readonly McpJwtService _jwt;
        private readonly McpAuthConfig _config;

        public McpAuthController(McpDataService data, McpEmailService email,
                                  McpJwtService jwt, McpAuthConfig config)
        {
            _data = data;
            _email = email;
            _jwt = jwt;
            _config = config;
        }

        [HttpGet("test")]
        public IActionResult Test() => Ok("controller ok");

        // ── GET: Cowork redirige aquí → devuelve pantalla de login como HTML
        [HttpGet("login_authorize")]
        public ContentResult Authorize(string response_type, string client_id,
                                       string redirect_uri, string state)
        {
            var baseUrl = _config.BaseUrl?.TrimEnd('/') ?? "";
            var html = LoginHtml(redirectUri: redirect_uri, state: state, error: null, baseUrl: baseUrl);
            return Content(html, "text/html");
        }

        // ── POST: Usuario envía email + contraseña
        [HttpPost("login_authorize")]
        public ContentResult AutenticarCredenciales([FromForm] string email,
                                                    [FromForm] string password,
                                                    [FromForm] string redirect_uri,
                                                    [FromForm] string state)
        {
            var baseUrl = _config.BaseUrl?.TrimEnd('/') ?? "";
            var usuario = _data.BuscarPorEmail(email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
                return Content(LoginHtml(redirect_uri, state, "Credenciales incorrectas.", baseUrl), "text/html");

            var codigoId = Guid.NewGuid();
            var codigo = new Random().Next(100000, 999999).ToString();
            _data.GuardarMfaCodigo(codigoId, usuario.McpUsuarioId, codigo, redirect_uri, state);

            _email.EnviarCodigoMfa(usuario.Email, usuario.Nombre, codigo);

            return Content(MfaHtml(codigoId.ToString(), error: null, baseUrl: baseUrl), "text/html");
        }

        // ── POST: Usuario envía código MFA

        [HttpPost("test-post")]
        public IActionResult TestPost() => Ok("post ok");

        [HttpPost("mfa")]
        public IActionResult ValidarMfa([FromForm] string codigo,
                                [FromForm] string sesion_id)
        {
            var baseUrl = _config.BaseUrl?.TrimEnd('/') ?? "";

            // Buscar sesión en BD en lugar de validar JWT
            if (!Guid.TryParse(sesion_id, out var sesionGuid))
                return Content(MfaHtml(sesion_id, "Sesión inválida.", baseUrl), "text/html");

            var sesion = _data.BuscarSesionMfa(sesionGuid);
            if (sesion == null)
                return Content(MfaHtml(sesion_id, "Sesión expirada. Inicia el proceso de nuevo.", baseUrl), "text/html");

            var usuarioId = (Guid)sesion.McpUsuarioId;
            var redirectUri = (string)sesion.RedirectUri;
            var state = (string)sesion.State;

            if (!_data.ValidarCodigoMfa(usuarioId, codigo))
                return Content(MfaHtml(sesion_id, "Código incorrecto o expirado.", baseUrl), "text/html");

            // MFA válido → emitir authorization code y redirigir a Cowork
            var authCode = Guid.NewGuid().ToString("N");
            _data.GuardarAuthCode(usuarioId, authCode, redirectUri);

            return Redirect($"{redirectUri}?code={authCode}&state={state}");
        }
        [HttpPost("token")]
        public IActionResult ObtenerToken([FromForm] string grant_type,
                                          [FromForm] string code,
                                          [FromForm] string redirect_uri,
                                          [FromForm] string? code_verifier,    // PKCE – Cowork lo manda siempre; aceptamos sin validar por ahora
                                          [FromForm] string? client_id)         // algunos clientes lo mandan en el body
        {
            if (grant_type != "authorization_code")
                return BadRequest(new { error = "unsupported_grant_type" });

            var (valido, usuarioId) = _data.ValidarAuthCode(code, redirect_uri);
            if (!valido || usuarioId == null)
                return BadRequest(new { error = "invalid_grant" });

            //var usuario = _data.BuscarPorEmail(""); // reemplazar por BuscarPorId
            var token = _jwt.GenerarAccessToken(usuarioId.Value, "", "");
            var tokenHash = _jwt.HashToken(token);
            _data.RegistrarToken(usuarioId.Value, tokenHash, _config.TokenHoras);

            return Ok(new
            {
                access_token = token,
                token_type = "Bearer",
                expires_in = _config.TokenHoras * 3600
            });
        }

        // ── POST: Revocar token
        [HttpPost("revoke")]
        public IActionResult RevocarToken([FromForm] string token)
        {
            var hash = _jwt.HashToken(token);
            // llamar sp_McpTokens_Revocar con el hash
            return Ok();
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegistrarCliente([FromBody] ClienteRegistroRequest body)
        {
            try
            {
                // Leer el body raw sin importar el Content-Type
                using var reader = new System.IO.StreamReader(Request.Body);
                var bodyRaw = await reader.ReadToEndAsync();
                var ruta = System.IO.Path.Combine(@"C:\SMDH\Logs", "mcp_register.txt");

                System.IO.File.AppendAllText(ruta,
           $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
           $"Headers: {string.Join(", ", Request.Headers.Select(h => $"{h.Key}={h.Value}"))}\n" +
           $"Body: {bodyRaw}\n\n");
            }
            catch { /* no interrumpir el flujo si falla el log */ }
            return Ok(new
            {
                client_id = "humaya-mcp-cowork",
                client_secret = (string)null,
                redirect_uris = body?.RedirectUris ?? new[] { "" },
                grant_types = new[] { "authorization_code" },
                response_types = new[] { "code" },
                token_endpoint_auth_method = "none"
            });
        }

        public class ClienteRegistroRequest
        {
            [JsonProperty("redirect_uris")]
            public string[] RedirectUris { get; set; }

            [JsonProperty("grant_types")]
            public string[] GrantTypes { get; set; }

            [JsonProperty("response_types")]
            public string[] ResponseTypes { get; set; }
        }
        // ════════════════════════════════
        //  HTML helpers (sin Razor)
        // ════════════════════════════════

        private static string LoginHtml(string redirectUri, string state, string error, string baseUrl) => $@"
<!DOCTYPE html><html lang='es'><head>
<meta charset='UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'>
<title>Humaya Digital – Acceso MCP</title>
<style>
  *{{box-sizing:border-box;margin:0;padding:0}}
  body{{font-family:Arial,sans-serif;background:#F4F5F5;
        display:flex;justify-content:center;align-items:center;height:100vh}}
  .card{{background:#fff;padding:40px;border-radius:8px;width:360px;
         box-shadow:0 2px 10px rgba(0,0,0,.1)}}
  h2{{color:#367C2B;margin-bottom:6px;font-size:20px}}
  .sub{{color:#666;font-size:12px;margin-bottom:24px}}
  label{{font-size:11px;font-weight:bold;color:#367C2B;
         text-transform:uppercase;display:block;margin-bottom:6px}}
  input{{width:100%;padding:10px;border:1px solid #ddd;border-radius:4px;
         font-size:14px;margin-bottom:16px}}
  button{{width:100%;padding:12px;background:#367C2B;color:#fff;
          border:none;border-radius:4px;font-weight:bold;font-size:14px;cursor:pointer}}
  button:hover{{background:#2d6322}}
  .error{{color:#C00000;font-size:12px;margin-bottom:12px;
          background:#FFF0F0;padding:8px;border-radius:4px;border-left:3px solid #C00000}}
</style></head><body>
<div class='card'>
  <h2>Humaya Digital MCP</h2>
  <div class='sub'>Ingresa tus credenciales para continuar</div>
  {(string.IsNullOrEmpty(error) ? "" : $"<div class='error'>{error}</div>")}
  <form method='post' action='{baseUrl}/mcpauth/oauth/login_authorize'>
    <input type='hidden' name='redirect_uri' value='{redirectUri}' />
    <input type='hidden' name='state'        value='{state}' />
    <label>Email</label>
    <input type='email' name='email' required autofocus />
    <label>Contraseña</label>
    <input type='password' name='password' required />
    <button type='submit'>Continuar →</button>
  </form>
</div></body></html>";

        private static string MfaHtml(string tempToken, string error, string baseUrl) => $@"
<!DOCTYPE html><html lang='es'><head>
<meta charset='UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'>
<title>Humaya Digital – Verificación</title>
<style>
  *{{box-sizing:border-box;margin:0;padding:0}}
  body{{font-family:Arial,sans-serif;background:#F4F5F5;
        display:flex;justify-content:center;align-items:center;height:100vh}}
  .card{{background:#fff;padding:40px;border-radius:8px;width:360px;
         box-shadow:0 2px 10px rgba(0,0,0,.1);text-align:center}}
  h2{{color:#367C2B;margin-bottom:8px;font-size:20px}}
  .sub{{color:#666;font-size:12px;margin-bottom:24px;line-height:1.6}}
  input{{width:100%;padding:14px;border:1px solid #ddd;border-radius:4px;
         font-size:30px;font-weight:bold;letter-spacing:14px;text-align:center;
         margin-bottom:16px}}
  button{{width:100%;padding:12px;background:#367C2B;color:#fff;
          border:none;border-radius:4px;font-weight:bold;font-size:14px;cursor:pointer}}
  button:hover{{background:#2d6322}}
  .error{{color:#C00000;font-size:12px;margin-bottom:12px;
          background:#FFF0F0;padding:8px;border-radius:4px;border-left:3px solid #C00000}}
</style></head><body>
<div class='card'>
  <h2>Verificación</h2>
  <div class='sub'>Ingresa el código de 6 dígitos<br>que enviamos a tu correo.</div>
  {(string.IsNullOrEmpty(error) ? "" : $"<div class='error'>{error}</div>")}
  <form method='post' action='{baseUrl}/mcpauth/oauth/mfa'>
    <input type='hidden' name='sesion_id' value='{tempToken}' />
    <input type='text' name='codigo' maxlength='6' pattern='[0-9]{{6}}'
           placeholder='000000' required autofocus autocomplete='one-time-code' />
    <button type='submit'>Verificar</button>
  </form>
</div></body></html>";
    }


}
