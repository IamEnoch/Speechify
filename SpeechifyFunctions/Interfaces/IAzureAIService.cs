using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechifyFunctions.Interfaces
{
    public interface IAzureAIService
    {
        /// <summary>
        /// Leverages document intelligence to extract text from the blob
        /// </summary>
        /// <param name="blobStream">Blob item stream</param>
        /// <returns></returns>
        public Task<string> ExtractTextFromBlobAsync(Stream blobStream);


        /// <summary>
        /// Leverages azure speech services to get create audio from text
        /// </summary>
        /// <param name="text">Text string</param>
        /// <returns></returns>
        public Task<byte[]> GenerateAudioFileAsync(string text);
       
    }
}
