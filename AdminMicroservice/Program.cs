
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", policy =>
    {
        // policy.WithOrigins("http://localhost:3000") // React dev server URL
        policy.WithOrigins()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // if using cookies/auth
    });


});
// Configure JWT Bearer Authentication (same as Customer/Identity service validation)
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});




// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin", "SuperAdmin"));
    
    options.AddPolicy("ManagerPolicy", policy =>
        policy.RequireRole("Admin", "SuperAdmin", "Manager"));
    
    options.AddPolicy("UserPolicy", policy =>
        policy.RequireRole("Admin", "SuperAdmin", "Manager", "User"));
});

// Add services for controllers
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Recommended in production

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
