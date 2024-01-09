using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SpeechifyFunctions.Models;
using SpeechifyFunctions.Interfaces;

namespace SpeechifyFunctions.Services.AzureBlobStorage
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private IConfiguration _configuration{ get; set; }
        public AzureBlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;            
        }
        public BlobStorageToken GetBlobSasToken(BlobContainerClient containerClient)
        {
            var key = new StorageSharedKeyCredential(_configuration["BlobStorageAccountName"],
                _configuration["BlobStorageAccountKey"]);
            var expiresOn = DateTime.Now.AddHours(2);
            var (sasToken, tokenExpiry) = (GetSasToken(containerClient, key, expiresOn, "b"), expiresOn);
            var token = new BlobStorageToken
            {
                ExpiresOn = tokenExpiry,
                Token = sasToken
            };
            return token;
        }
        public string GetSasToken(BlobContainerClient container, StorageSharedKeyCredential key, DateTimeOffset expireOn, string sasBuilderResource, string? storedPolicyName = null)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = container.Name,
                Resource = sasBuilderResource
            };
            if (storedPolicyName == null)
            {
                sasBuilder.StartsOn = DateTimeOffset.UtcNow;
                sasBuilder.ExpiresOn = expireOn;
                sasBuilder.SetPermissions(BlobContainerSasPermissions.All);
            }
            else
            {
                sasBuilder.Identifier = storedPolicyName;
            }
            // Use the key to get the SAS token.
            return sasBuilder.ToSasQueryParameters(key).ToString();
        }
    }
}
