using EasyCoreAPI.Extensions;
using EasyCoreAPI.Options;
using MongoODM.Net.Api.Models;
using MongoODM.Net.Api.Services;
using MongoODM.Net.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration=builder.Configuration;
var services=builder.Services;

//
services.AddEndpointsApiExplorer();
services.AddAPIControllers();
services.AddAPIVersioning(1);
services.AddAPISwaggerGen(configuration, AuthScheme.Bearer);
services.AddMongoDbContext(configuration);

//services
services.AddScoped<ICustomerService, CustomerService>();

//Add Collections
services.AddMongoRepository<Customer>("ExampleDb");
services.AddMongoRepository<Inventory>("InventoryDb");

var app = builder.Build();


app.UseRouting();
app.UseAPISwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();