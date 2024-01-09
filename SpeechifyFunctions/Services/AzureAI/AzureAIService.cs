using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SpeechifyFunctions.Interfaces;

namespace SpeechifyFunctions.Services.Azure_Ai
{
    public class AzureAIService : IAzureAIService
    {
        private readonly IConfiguration _configuration;
        public AzureAIService(IConfiguration configuration)
        {
            _configuration = configuration;                        
        }
        /// <summary>
        /// Leverages document intelligence to extract text from the blob
        /// </summary>
        /// <param name="blobStream">Blob item stream</param>
        /// <returns></returns>
        public async Task<string> ExtractTextFromBlobAsync(Stream blobStream)
        {
            // Create a temporary file to store the blob contents
            //var tempFilePath = Path.GetTempFileName();
            //var fileStream = new FileStream(tempFilePath, FileMode.OpenOrCreate);
            //await blobStream.CopyToAsync(fileStream);

            // Create the Form Recognizer client
            string endpoint = _configuration["TextEndpoint"]!;
            string key = _configuration["TextKey"]!;
            var credential = new AzureKeyCredential(key);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);


            // Analyze the document and extract the text
            AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-read", blobStream);
            AnalyzeResult result = operation.Value;
            var text = result.Content;

            // Delete the temporary file
            //File.Delete(tempFilePath);

            return text;
        }

        /// <summary>
        /// Leverages azure speech services to get create audio from text
        /// </summary>
        /// <param name="text">Text string</param>
        /// <returns></returns>
        public async Task<byte[]> GenerateAudioFileAsync(string text)
        {
            string subscriptionKey = _configuration["SpeechSubscriptionKey"]!;
            string region = _configuration["SpeechRegion"]!;
            var speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);

            // The language of the voice that speaks.
            speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";

            var speechSynthesizer = new SpeechSynthesizer(speechConfig);

            var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);

            return speechSynthesisResult.AudioData;
        }
    }
}
