using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using APIGateWay.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Ocelot configuration from ocelot.json
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
// Add permissions configuration
builder.Configuration.AddJsonFile("permissions-config.json", optional: false, reloadOnChange: true);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register services
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IAuditService, AuditService>();

// Add controllers for audit endpoints
builder.Services.AddControllers();

// Add session support for audit logging
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Enable CORS
app.UseCors("AllowReactApp");

// Add session middleware
app.UseSession();

// Add audit middleware
app.UseMiddleware<APIGateWay.Middleware.AuditMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Map audit controller
app.MapControllers();

await app.UseOcelot();

app.Run();
//// Configure JWT authentication
//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]); // Get secret key from appsettings.json
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer("GatewayAuthenticationKey", option =>
//    {
//        option.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = jwtSettings["Jwt:Issuer"],
//            ValidAudience = jwtSettings["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Jwt:Key"]))
//        };
//    });

//builder.Services.AddOcelot(builder.Configuration);

//var app = builder.Build();

////app.UseAuthentication();
////app.UseAuthorization();
//await app.UseOcelot();

//app.Run();