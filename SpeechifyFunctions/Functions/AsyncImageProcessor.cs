using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpeechifyFunctions.Enums;
using SpeechifyFunctions.Interfaces;
using SpeechifyFunctions.Services.Azure_Ai;
using SpeechifyFunctions.Services.AzureStorage;

namespace SpeechifyFunctions.Functions
{
    public class AsyncImageProcessor
    {
        private readonly ILogger<AsyncImageProcessor> _logger;
        private readonly IConfiguration _configuration;

        public AsyncImageProcessor(ILogger<AsyncImageProcessor> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// AsyncImageProcessor
        /// </summary>
        /// <param name="inputBlob">Input blob</param>
        /// <param name="blobId">blob Id(GUID)</param>
        /// <param name="context"></param>
        /// <returns></returns>
        [Function(nameof(AsyncImageProcessor))]
        [BlobOutput("audio-output/{blobId}-output.wav", Connection = "SpeechifyStorageConnectionString")]
        public async Task<byte[]> Run(
            [BlobTrigger("images/{blobId}", Connection = "SpeechifyStorageConnectionString")] byte[] inputBlob,
            string blobId,
            FunctionContext context)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {blobId} \n Data: {inputBlob}\n");
            string? tableName = "";
            var azureStorageService = new AzureStorageService(_configuration);
            var azureAiService = new AzureAIService(_configuration);

            try
            {
                
                //Update the value of the executing process
                tableName = await azureStorageService.CreateAzureTableAsync();
                _logger.LogInformation($"Created table: {tableName}\n");

                await azureStorageService.AddEntityToTableAsync(blobId, tableName, StatusEnum.Processing);
                _logger.LogInformation($"Add entity to table:\n");

                //Create a stream
                MemoryStream inputBlobStream = new MemoryStream(inputBlob);

                _logger.LogInformation($"Function created a stream from the input blob\n");

                // Extract text from the blob
                var text = await azureAiService.ExtractTextFromBlobAsync(inputBlobStream);
                _logger.LogInformation($"Extracted text from blob: {text}\n");

                // Generate the audio file
                var audioFile = await azureAiService.GenerateAudioFileAsync(text);
                _logger.LogInformation($"Generated audio file with length:\n {audioFile.Length}\n");

                await azureStorageService.UpdateEntityToTableAsync(blobId, tableName, StatusEnum.Complete);
                return audioFile;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception thrown is {ex}");
                await azureStorageService.UpdateEntityToTableAsync(blobId, tableName, StatusEnum.Failed);
                throw;
            }
        }
    }
}
