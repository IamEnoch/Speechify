using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;
using SpeechifyApi.Interfaces;
using SpeechifyApi.Models;

namespace SpeechifyApi.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;
        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<BlobContainerClient> CreateContainerAsync(string containerName)
        {
            //Connection string of the storage account
            var connString = _configuration.GetConnectionString("BlobStorageConnectionString");

            var containerClient = new BlobContainerClient(connString, containerName);
            await containerClient.CreateIfNotExistsAsync();

            var expiresOn = DateTime.Now.AddDays(1);
            var key = new StorageSharedKeyCredential(_configuration["BlobStorageAccountName"],
                _configuration["BlobStorageAccountKey"]);

            var sasToken = GetSasToken(containerClient, key, expiresOn, "c");
            var containerUrl = $"{containerClient.Uri}/?{sasToken}";
            var tokenExpiry = expiresOn;
            var accountEndpoint = containerClient.Uri.ToString().Replace($"/{containerName}", "");
            var connectionString = $"BlobEndpoint={accountEndpoint}/;SharedAccessSignature={sasToken}";

            //new BlobContainer(containerClient.Name, containerUrl, connectionString, tokenExpiry.ToString());
            return containerClient;
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


        
        /*public ArtImage GetArtImages(string containerName, string uniqueStorageName, string artistId)
        {
            var connectionString = _configuration.GetConnectionString("BlobStorageConnectionString");
            var containerClient = new BlobContainerClient(connectionString, containerName);
            containerClient.CreateIfNotExists();
            var blobStorageToken = GetBlobSasToken(containerClient);
            var prefix = $"{artistId}/{uniqueStorageName}";
            var blobItems = containerClient.GetBlobs(prefix: prefix).Where(i => i.Name.Contains($"{prefix}"));
            var artImages = blobItems
                .Select(blobItem => $"{containerClient.Uri}/{blobItem.Name}?{blobStorageToken.Token}")
                .ToList();
            return new ArtImage
            {
                ImageUrl = artImages,
                ExpiresOn = blobStorageToken.ExpiresOn
            };
        }*/

    }
}

