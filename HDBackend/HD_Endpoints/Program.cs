using HD.Endpoints.Middleware;
using HD.Security;
using Irony.Parsing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using Quobject.EngineIoClientDotNet.Client.Transports;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Text;
using WebSocket4Net;

QuestPDF.Settings.License = LicenseType.Community;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Login"]))
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

builder.Services.AddWebSockets(options => //codigo socket
{
    options.KeepAliveInterval = TimeSpan.FromMinutes(2);
}); //codigo socket

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); //codigo socket
}
app.UseHttpsRedirection();
app.UseRouting();
//app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ManejadorMiddlewares>();
app.UseCors("corsApp");
app.UseCors("CorsPolicy"); //codigo socket
app.UseWebSockets(); //codigo socket

app.Use(async (context, next) => //codigo socket
{
    if (context.Request.Path == "/ws") //codigo socket
    {
        if (context.WebSockets.IsWebSocketRequest) //codigo socket
        {
            var token = context.Request.Query["token"]; //codigo socket
            if (!ValidateToken(token, builder.Configuration)) //codigo socket
            {   
                context.Response.StatusCode = 401; //codigo socket
                await context.Response.WriteAsync("Invalid token"); //codigo socket
                return; //codigo socket
            }
            var webSocket = await context.WebSockets.AcceptWebSocketAsync(); //codigo socket
            await HandleWebSocket(context, webSocket); //codigo socket
        }
        else 
        {
            context.Response.StatusCode = 400; //codigo socket
        }
    }
    else
    {
        await next(); //codigo socket
    }
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapFallbackToController("Index", "Home");
});

app.UseMiddleware<HD.Endpoints.Middleware.WebSocketMiddleware>(); //codigo socket
app.MapControllers(); //codigo socket


app.Run();

bool ValidateToken(string token, IConfiguration configuration) //codigo socket
{
    if (string.IsNullOrEmpty(token)) return false; //codigo socket

    var mySecret = configuration["Jwt:Key"]; //codigo socket
    var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret)); //codigo socket

    var tokenHandler = new JwtSecurityTokenHandler(); //codigo socket

    try //codigo socket
    {
        tokenHandler.ValidateToken(token, new TokenValidationParameters //codigo socket
        {
            ValidateIssuerSigningKey = true, //codigo socket
            IssuerSigningKey = mySecurityKey, //codigo socket
            ValidateIssuer = false, //codigo socket
            ValidateAudience = false //codigo socket
        }, out SecurityToken validatedToken); //codigo socket

        return true; //codigo socket
    }
    catch //codigo socket
    {
        return false; //codigo socket
    }
}

async Task HandleWebSocket(HttpContext context, System.Net.WebSockets.WebSocket webSocket) //codigo socket
{
    var buffer = new byte[1024 * 4]; //codigo socket
    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None); //codigo socket

    while (!result.CloseStatus.HasValue) //codigo socket
    { 
        await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None); //codigo socket
        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None); //codigo socket
    }

    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None); //codigo socket
}