using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpeechifyFunctions.Interfaces;
using SpeechifyFunctions.Services.Azure_Ai;
using SpeechifyFunctions.Services.AzureBlobStorage;
using SpeechifyFunctions.Services.AzureStorage;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        // Load user secrets
        config.AddUserSecrets<Program>(); // Replace YourStartupType with your actual Startup type
        // Load settings from local.settings.json
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

    })
    .ConfigureServices((context, services) =>
    {
        // Add services
        services.AddSingleton<IAzureStorageService, AzureStorageService>();
        services.AddSingleton<IAzureAIService, AzureAIService>();
        services.AddSingleton<IAzureBlobStorageService, AzureBlobStorageService>();
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();