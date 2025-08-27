using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Entities;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ConUser") ?? throw new InvalidOperationException("Connection string 'pcon' not found.");
builder.Services.AddDbContext<ScmdbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
                    .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Required for JWT authentication
    });
});


builder.Services.AddAuthorization();
builder.Services.AddControllers(); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAll"); // Enable CORS policy
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
