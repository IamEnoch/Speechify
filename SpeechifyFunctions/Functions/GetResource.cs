using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpeechifyFunctions.Models;
using SpeechifyFunctions.Services.AzureStorage;
using SpeechifyFunctions.Services.AzureBlobStorage;
using SpeechifyFunctions.Interfaces;

namespace SpeechifyFunctions.Functions
{
    public class GetResource
    {
        private readonly ILogger _logger;
        private IConfiguration _configuration;

        public GetResource(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetResource>();
            _configuration = configuration;
        }

        /// <summary>
        /// GetResource
        /// </summary>
        /// <param name="req">HTTP request data</param>
        /// <param name="blobClient">Blob client</param>
        /// <param name="blobId">blob Id(GUID)</param>
        /// <param name="context"></param>
        /// <returns></returns>
        [Function("GetResource")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetResource/{blobId}")] HttpRequest req,
            [BlobInput("audio-output/{blobId}.jpeg-output.wav", Connection = "SpeechifyStorageConnectionString")] BlockBlobClient blobClient,
            string blobId,
            FunctionContext context)
        {

            _logger.LogInformation($"C# HTTP trigger function processed a request for status on {blobId}");

            //Connection string of the storage account
            var connString = _configuration.GetConnectionString("SpeechifyStorageConnectionString");

            var containerClient = new BlobContainerClient(connString, "audio-output");
            await containerClient.CreateIfNotExistsAsync();

            var expiresOn = DateTime.Now.AddDays(1);
            var key = new StorageSharedKeyCredential(_configuration["BlobStorageAccountName"],
                _configuration["BlobStorageAccountKey"]);

            var azureBlobStorageService = new AzureBlobStorageService(_configuration);
            var sasToken = azureBlobStorageService.GetSasToken(containerClient, key, expiresOn, "c");

            var response = new StatusResponse()
            {
                Link = $"{blobClient.Uri.AbsoluteUri}?{sasToken}",
                StatusCode = StatusCodes.Status200OK.ToString(),
            };

            return new OkObjectResult(response);
        }
    }
}
