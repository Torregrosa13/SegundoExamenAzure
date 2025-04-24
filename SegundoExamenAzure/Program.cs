using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using SegundoExamenAzure.Data;
using SegundoExamenAzure.Helpers;
using SegundoExamenAzure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<HelperUsuarioToken>();
builder.Services.AddTransient<RepositoryCubos>();
builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient = builder.Services.BuildServiceProvider()
    .GetService<SecretClient>();
string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddDbContext<CubosContext>(options =>
{
    options.UseSqlServer(connectionString);
});


KeyVaultSecret secretKeySecret = await secretClient.GetSecretAsync("SecretKey");
string secretKey = secretKeySecret.Value;
HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration, secretKey);
builder.Services.AddSingleton<HelperActionServicesOAuth>(provider => helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema())
   .AddJwtBearer(helper.GetJwtBearerOptions());



builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.MapOpenApi();
app.UseHttpsRedirection();
app.UseSwaggerUI(
    options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Api Segundo Examen Cubos");
        options.RoutePrefix = "";
    });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
