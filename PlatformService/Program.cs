using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsProduction())
{
    foreach (var key in Environment.GetEnvironmentVariables().Keys)
    {
        Console.WriteLine($"{key}");
    }

    Console.WriteLine($"all variables were printed!");
    string dbPassword = Environment.GetEnvironmentVariable("SA_PASSWORD");
    string passwordStr = $"Password={dbPassword};";


    Console.WriteLine("Password is " + passwordStr);
    string finalConStr = builder.Configuration.GetConnectionString("PlatformsConn") + passwordStr + "TrustServerCertificate=true;";

    Console.WriteLine("-->connection string is " + finalConStr);
    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(finalConStr));
}
else
{
    Console.WriteLine("-->Using InMem DB");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
}



builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

Console.WriteLine($"--> CommandService Endpoint {app.Configuration["CommandService"]}");

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

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
