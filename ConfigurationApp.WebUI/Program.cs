using ConfigurationApp.LibraryLayer;
using ConfigurationApp.StorageLayer;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add IStorageProvider and ConfigurationReader to DI container
// Add IStorageProvider and ConfigurationReader to DI container
// Add IStorageProvider and ConfigurationReader to DI container
builder.Services.Configure<ConfigurationAppSettings>(builder.Configuration.GetSection("ConfigurationAppSettings"));

builder.Services.AddTransient<IStorageProvider, MsSqlStorageProvider>(provider =>
{
    var configuration = provider.GetRequiredService<IOptions<ConfigurationAppSettings>>().Value;
    var connectionString = configuration.DefaultConnection;
    return new MsSqlStorageProvider(connectionString);
});

builder.Services.AddTransient<ConfigurationReader>(provider =>
{
    var configuration = provider.GetRequiredService<IOptions<ConfigurationAppSettings>>().Value;
    var connectionString = configuration.DefaultConnection;
    var applicationName = configuration.ApplicationName;
    var refreshTimerIntervalInMs = configuration.RefreshTimerIntervalInMs;
    var storageProvider = provider.GetRequiredService<IStorageProvider>();

    return new ConfigurationReader(applicationName, connectionString, refreshTimerIntervalInMs, storageProvider);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
