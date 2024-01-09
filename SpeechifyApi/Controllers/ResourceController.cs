using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SpeechifyApi.Interfaces;
using SpeechifyApi.Models;
using SpeechifyApi.Services;

namespace SpeechifyApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IConfiguration _configuration;
        public ResourceController(IConfiguration configuration, IBlobStorageService blobStorageService, HttpClient httpClient)
        {
            _configuration = configuration;
            _blobStorageService = blobStorageService;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Uploads a file to blob storage
        /// </summary>
        /// <returns></returns>

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync()
        {
            if (Request.Body == null || Request.ContentLength == 0)
            {
                return BadRequest("Request body is empty.");
            }

            var containerName = "images";

            var containerClient = await _blobStorageService.CreateContainerAsync(containerName);

            containerClient.CreateIfNotExists();

            // Generate a unique file name, or use a specific one based on your needs.
            var blobId = Guid.NewGuid().ToString();
            var fileName = $"{blobId}.jpeg";

            //Should handle the upload using an azure function
            var result = await containerClient.UploadBlobAsync(fileName, Request.Body);

            //get api url from appsettings.json
            var apiUrl = await _configuration["ApiUrl"];

            //Add location header that the client can use to poll the status of execution
            return Accepted($"{apiUrl}/api/Resource/CheckStatus/{blobId}", new UploadResponse
            {
                StatusCode = StatusCodes.Status202Accepted,
                StatusMessage = StatusCodes.Status202Accepted.ToString(),
                StatusUrl = $"api/status/{blobId}"

            });
        }

        /// <summary>
        /// Checks the status of the request
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        [HttpGet("CheckStatus/{processId}")]
        public async Task<IActionResult> CheckStatus(string processId)
        {

            var statusResponse = await _httpClient.GetAsync($"{_configuration["ApiUrl"]}/api/RequestStatus/{processId}");

            if (statusResponse.IsSuccessStatusCode)
            {
                var contentString = await statusResponse.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<RequestStatusResponse>(contentString);

                if (responseData != null)
                {
                    if (responseData.StatusCode == StatusCodes.Status102Processing)
                    {
                        return Ok();
                    }
                    else if (responseData.StatusCode == StatusCodes.Status200OK)
                    {
                        //Redirect the user to the endpoint that has the link to the resource
                        //For now we can give the user the link to the resource found in blob storage then handle redirection later\
                        return Redirect(
                            $"{_configuration["ApiUrl"]}/api/RequestStatus/{processId}");
                    }
                    else
                    {
                        return NotFound();
                    }
                }

                return NotFound();
            }
            return NotFound();
        }
    }
}
