using ControllProjects.Models;
using Microsoft.Azure.Cosmos;
using ControllProjects.Interfaces;
using ControllProjects.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(builder.Configuration).GetAwaiter().GetResult());

static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfiguration configuration)
{
    string databaseName = configuration["CosmosDb:DatabaseName"];
    string containerName = configuration["CosmosDb:ContainerName"];
    string account = configuration["CosmosDb:Account"];
    string key = configuration["CosmosDb:Key"];
    CosmosClient client = new CosmosClient(account, key);
    CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
    return cosmosDbService;
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
