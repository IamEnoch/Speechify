using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;
using SpeechifyFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechifyFunctions.Interfaces
{
    public interface IAzureBlobStorageService
    {
        public BlobStorageToken GetBlobSasToken(BlobContainerClient containerClient);

        public string GetSasToken(BlobContainerClient container, StorageSharedKeyCredential key, DateTimeOffset expireOn, string sasBuilderResource, string? storedPolicyName = null);

    }
}
