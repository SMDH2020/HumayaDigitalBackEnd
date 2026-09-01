using HD.Endpoints.Middleware;
using HD.Generales.MCP.Config;
using HD.Generales.MCP.Security;
using HD.Generales.MCP.Services;
using HD.Endpoints.Controllers.MCP.Tools;
using HD.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using System.Text;

QuestPDF.Settings.License = LicenseType.Community;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 MB
});

// Register HttpClient
builder.Services.AddHttpClient();

builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Login"])),
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Token Caducado" });
                    return context.Response.WriteAsync(result);
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddCors(o =>
{
    o.AddPolicy("corsApp", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});
builder.Services.AddScoped<ISesion, Sesion>();

// ── MCP: servicios de autenticación exclusivos (independientes del JWT de la API) ──
var mcpConfig = builder.Configuration.GetSection("McpAuth").Get<McpAuthConfig>();
builder.Services.AddSingleton(mcpConfig);
builder.Services.AddScoped<McpDataService>();
builder.Services.AddScoped<McpEmailService>();
builder.Services.AddScoped<McpJwtService>();

// ── MCP: servidor de herramientas (expone /mcp) ──
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<PresentacionesTools>()
    .WithTools<FinanzasTools>()
    .WithTools<VendedoresTools>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("corsApp");
app.UseAuthentication();
app.UseAuthorization();

// ── MCP: metadata OAuth ──────────────────────────────────────────────────────────────────
// Flujo que sigue Cowork para descubrir el authorization_endpoint:
//
//  1. GET /mcp  →  401  WWW-Authenticate: Bearer resource_metadata="…/mcp/.well-known/oauth-protected-resource"
//  2. GET /mcp/.well-known/oauth-protected-resource  →  { authorization_servers: ["https://…"] }
//  3. GET /.well-known/oauth-authorization-server    →  { authorization_endpoint: "…/mcpauth/oauth/login_authorize" }
//  4. Cowork abre el authorization_endpoint en el browser del usuario
// ──────────────────────────────────────────────────────────────────────────────────────────

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;

    // Helper para obtener la base URL.
    // Prioridad: 1) McpAuth:BaseUrl en appsettings  2) X-Forwarded headers  3) Request.Host
    string GetBase()
    {
        if (!string.IsNullOrWhiteSpace(mcpConfig?.BaseUrl))
            return mcpConfig.BaseUrl.TrimEnd('/');

        var scheme = context.Request.Headers["X-Forwarded-Proto"].FirstOrDefault()
                     ?? context.Request.Scheme;
        var host   = context.Request.Headers["X-Forwarded-Host"].FirstOrDefault()
                     ?? context.Request.Host.Value;
        return $"{scheme}://{host}";
    }

    // ── Paso 2: Protected Resource Metadata (RFC 9728) ──────────────────────────────────
    // Cowork llega aquí porque el 401 de McpAuthenticationMiddleware lo indica.
    // Le decimos cuál es la URL del authorization server (la raíz del servidor).
    if (path == "/mcp/.well-known/oauth-protected-resource")
    {
        var base_url = GetBase();
        context.Response.Headers["Cache-Control"] = "no-store, no-cache";
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            resource               = $"{base_url}/mcp",
            authorization_servers  = new[] { base_url },   // raíz → Cowork busca /.well-known/oauth-authorization-server aquí
            bearer_methods_supported = new[] { "header" }
        });
        return;
    }

    // ── Paso 3a: Authorization Server Metadata en la RAÍZ (RFC 8414) ────────────────────
    // Cowork fetcha /.well-known/oauth-authorization-server (sin /mcp) para saber
    // a qué URL mandar al usuario a autenticarse.
    if (path == "/.well-known/oauth-authorization-server")
    {
        var base_url = GetBase();
        context.Response.Headers["Cache-Control"] = "no-store, no-cache";
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            issuer                                = base_url,
            authorization_endpoint                = $"{base_url}/mcpauth/oauth/login_authorize",
            token_endpoint                        = $"{base_url}/mcpauth/oauth/token",
            revocation_endpoint                   = $"{base_url}/mcpauth/oauth/revoke",
            registration_endpoint                 = $"{base_url}/mcpauth/oauth/register",
            response_types_supported              = new[] { "code" },
            grant_types_supported                 = new[] { "authorization_code" },
            code_challenge_methods_supported      = new[] { "S256" },
            token_endpoint_auth_methods_supported = new[] { "none" }
        });
        return;
    }

    // ── Paso 3b: También respondemos bajo /mcp/.well-known/ (compatibilidad) ────────────
    if (path == "/mcp/.well-known/oauth-authorization-server")
    {
        var base_url = GetBase();
        context.Response.Headers["Cache-Control"] = "no-store, no-cache";
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            issuer                                = $"{base_url}/mcp",
            authorization_endpoint                = $"{base_url}/mcpauth/oauth/login_authorize",
            token_endpoint                        = $"{base_url}/mcpauth/oauth/token",
            revocation_endpoint                   = $"{base_url}/mcpauth/oauth/revoke",
            registration_endpoint                 = $"{base_url}/mcpauth/oauth/register",
            response_types_supported              = new[] { "code" },
            grant_types_supported                 = new[] { "authorization_code" },
            code_challenge_methods_supported      = new[] { "S256" },
            token_endpoint_auth_methods_supported = new[] { "none" }
        });
        return;
    }

    await next();
});

// ── MCP: valida Bearer token MCP antes de servir las tools ──
app.UseMiddleware<McpAuthenticationMiddleware>();

app.UseMiddleware<ManejadorMiddlewares>();

// ── MCP: redirect /authorize → nuestro login (Claude.ai usa el path estándar OAuth) ──
// IMPORTANTE: usar URL completa con BaseUrl para no perder el path /mh/apihumayadigital/
app.MapGet("/authorize", (HttpContext ctx) =>
{
    var query = ctx.Request.QueryString.Value ?? "";
    // Construir URL completa: BaseUrl del config O scheme+host+PathBase del request
    var baseUrl = !string.IsNullOrWhiteSpace(mcpConfig?.BaseUrl)
        ? mcpConfig.BaseUrl.TrimEnd('/')
        : $"{ctx.Request.Scheme}://{ctx.Request.Host}{ctx.Request.PathBase}";
    return Results.Redirect($"{baseUrl}/mcpauth/oauth/login_authorize{query}");
});

// ── MCP: endpoint del servidor de herramientas ──
app.MapMcp("/mcp");

app.MapControllers();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



