using System.ComponentModel.DataAnnotations;
using netdetective.IpTests;
using netdetective.Controllers;
using netdetective.Auth;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IpInfoSettings>(configuration.GetSection("IpInfoSettings"));
builder.Services.Configure<IpTestControllerSettings>(configuration.GetSection("IpTestControllerSettings"));
builder.Services.Configure<ConnectionValidationSettings>(configuration.GetSection("Auth"));

builder.Services.AddSingleton<IIpTestFactory, IpInfoFactory>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(configuration.GetSection("Kestrel"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


public class IpInfoSettings
{
    [Required]
    public string VpnListPath { get; set; } = default!;

    [Required]
    public string DatacenterListPath { get; set; } = default!;

    [Required]
    public string DnsBlDatabasePath { get; set; } = default!;
}
