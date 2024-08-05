using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using ChatApi.Interfaces;
using ChatApi.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(configuration =>
    {
            var config = configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

            var builtConfig = config.Build();
    })
    .ConfigureServices(services =>
    {
        var cosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
        var options = new CosmosClientOptions()
        {
            SerializerOptions = new CosmosSerializationOptions()
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            }
        };
        var client = new CosmosClient(cosmosDbConnectionString, options);
        var cosmosDbService = new CosmosDbService(client, "chat", "chatsessions", "chatmessages");
        services.AddSingleton<ICosmosDbService>(cosmosDbService);        
    })    
    .Build();

host.Run();
