using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register Scmdbcontext with connection string from configuration
//builder.Services.AddDbContext<DBSCM.Context.SCMDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ConSCM")));

// Register generic repository for all entities
//builder.Services.AddScoped(typeof(DBSCM.Repository.IRepository<>), typeof(DBSCM.Repository.Repository<>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
