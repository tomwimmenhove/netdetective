using netdetective.IpTests;
using netdetective.Controllers;
using netdetective.Auth;
using netdetective.RequestInfoProviders;
using Microsoft.OpenApi.Models;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IpInfoSettings>(configuration.GetSection("IpInfoSettings"));
builder.Services.Configure<IpTestControllerSettings>(configuration.GetSection("IpTestControllerSettings"));
builder.Services.Configure<ConnectionValidationSettings>(configuration.GetSection("Auth"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRapidApiRequestInfoProvider, RapidApiRequestInfoProvider>();
builder.Services.AddSingleton<IIpQuerier, IpQuerier>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetDetective", Version = "v1" });

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "NetDetective.xml");
         c.IncludeXmlComments(filePath);
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(configuration.GetSection("Kestrel"));
});

var app = builder.Build();

app.UseMiddleware<ConnectionValidation>();

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
