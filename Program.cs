using Microsoft.EntityFrameworkCore;
using socializer.Core.Datasets;
using socializer.Core.Datasets.Analyzers;
using socializer.Data.Domain.Interfaces;
using socializer.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite("Data Source=socializer.db"),
    ServiceLifetime.Scoped);
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDatasetDomainService, DatasetDomainService>();
builder.Services.AddScoped<IAverageConnectionsAtDistancesDatasetAnalyzer, AverageConnectionsAtDistancesDatasetAnalyzer>();
builder.Services.AddScoped<IAverageCliqueSizeDatasetAnalyzer, AverageCliqueSizeDatasetAnalyzer>();
builder.Services.AddScoped<IAverageNumberOfConnectionsPerPeerAnalyzer, AverageNumberOfConnectionsPerPeerAnalyzer>();
builder.Services.AddScoped<INumberOfPeersAnalyzer, NumberOfPeersAnalyzer>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (builder.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("../swagger/v1/swagger.json", "DemoAPI v1");
    });
}

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<DataContext>().Database.EnsureCreatedAsync();
}

app.Run();