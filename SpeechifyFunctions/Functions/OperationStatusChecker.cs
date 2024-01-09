using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpeechifyFunctions.Enums;
using SpeechifyFunctions.Interfaces;
using SpeechifyFunctions.Models;
using SpeechifyFunctions.Services.AzureStorage;

namespace SpeechifyFunctions.Functions
{
    public class OperationStatusChecker
    {
        private readonly ILogger<OperationStatusChecker> _logger;
        private readonly IConfiguration _configuration;

        public OperationStatusChecker(ILogger<OperationStatusChecker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// AsyncOperationStatusChecker
        /// </summary>
        /// <param name="req">HTTP request data</param>
        /// <param name="blobClient">Blob client</param>
        /// <param name="blobId">blob Id(GUID)</param>
        /// <param name="context"></param>
        /// <returns></returns>
        [Function(nameof(OperationStatusChecker))]
        public  IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RequestStatus/{blobId}")] HttpRequest req,
            [BlobInput("audio-output/{blobId}.jpeg-output.wav", Connection = "SpeechifyStorageConnectionString")] BlockBlobClient blobClient,
            string blobId,
            FunctionContext context)
        {

            _logger.LogInformation($"C# HTTP trigger function processed a request for status on {blobId}");

            var azureStorageService = new AzureStorageService(_configuration);
            var processResponse = azureStorageService.GetStatusOfProcess(blobId);

            if (processResponse == null)
            {
                return new NotFoundResult();
            }

            if (processResponse.Message == StatusEnum.Processing)
            {
                var response = new StatusResponse()
                {
                    StatusCode = StatusCodes.Status102Processing.ToString(),
                };
                var responseObject = new ObjectResult(response);
                responseObject.StatusCode = StatusCodes.Status102Processing;
                return responseObject;
            }
            else if (processResponse.Message == StatusEnum.Complete)
            {
                var response = new StatusResponse()
                {
                    Link = blobClient.Uri.AbsoluteUri,
                    StatusCode = StatusCodes.Status200OK.ToString(),
                };

                return new OkObjectResult(response);
            }

            return new NotFoundResult();
        }
    }
}
