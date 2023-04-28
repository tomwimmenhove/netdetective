using netdetective.IpTests;
using netdetective.Controllers;
using netdetective.Auth;
using netdetective.RequestInfoProviders;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IpInfoSettings>(configuration.GetSection("IpInfoSettings"));
builder.Services.Configure<IpTestControllerSettings>(configuration.GetSection("IpTestControllerSettings"));
builder.Services.Configure<ConnectionValidationSettings>(configuration.GetSection("Auth"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRapidApiRequestInfoProvider, RapidApiRequestInfoProvider>();
builder.Services.AddSingleton<IIpTestFactory, IpInfoFactory>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
