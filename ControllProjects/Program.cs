using ControllProjects.Models;
using Microsoft.Azure.Cosmos;
using ControllProjects.Interfaces;
using ControllProjects.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string cosmosDbConnectionString = builder.Configuration.GetConnectionString("CosmosDBConnectionString");
var cosmosClient = new CosmosClient(cosmosDbConnectionString);
string databaseName = builder.Configuration["CosmosDB:CosmosDatabaseId"];
string containerName = builder.Configuration["CosmosDB:CosmosContainerId"];
builder.Services.AddSingleton<ICosmosDbService>(new CosmosDbService(cosmosClient, databaseName, containerName));

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
