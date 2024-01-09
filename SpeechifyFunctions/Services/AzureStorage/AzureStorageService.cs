using Azure.Data.Tables;
using Azure;
using Microsoft.Extensions.Configuration;
using SpeechifyFunctions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeechifyFunctions.Models;
using SpeechifyFunctions.Interfaces;

namespace SpeechifyFunctions.Services.AzureStorage
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly IConfiguration _configuration;
        public AzureStorageService(IConfiguration configuration)
        {

            _configuration = configuration;

        }
        /// <summary>
        /// CreateAzureTableAsync
        /// </summary>
        /// <returns>Name of the table</returns>
        public async Task<String> CreateAzureTableAsync()
        {
            // Create the table client.
            var connectionString = _configuration.GetConnectionString("SpeechifyCosmosDbConnectionString");
            var tableClient = new TableClient(connectionString, "ProcessStatus");

            // Create the table if it doesn't exist.
            await tableClient.CreateIfNotExistsAsync();

            return tableClient.Name;

        }

        /// <summary>
        /// Add an entity to the table
        /// </summary>
        /// <param name="processId">Process id created using the blob id</param>
        /// <param name="tableName">Table name</param>
        /// <param name="status">Status of the operation</param>
        /// <returns></returns>
        public async Task AddEntityToTableAsync(string processId, string tableName, StatusEnum status)
        {
            var connectionString = _configuration.GetConnectionString("SpeechifyCosmosDbConnectionString");
            var tableClient = new TableClient(connectionString, tableName);

            // Create a new entity.
            var entity = new Status()
            {
                RowKey = processId.Replace(".jpeg", ""),
                Message = status
            };

            // Add values to the entity.
            await tableClient.AddEntityAsync<Status>(entity);

        }

        /// <summary>
        /// Update entity in the table
        /// </summary>
        /// <param name="processId">Process id created using the blob id</param>
        /// <param name="tableName">Table name</param>
        /// <param name="status">Status of the operation</param>
        /// <returns></returns>
        public async Task UpdateEntityToTableAsync(string processId, string tableName, StatusEnum status)
        {
            var connectionString = _configuration.GetConnectionString("SpeechifyCosmosDbConnectionString");
            var tableClient = new TableClient(connectionString, tableName);

            // Retrieve the entity to update
            var entityToUpdate = await tableClient.GetEntityAsync<Status>(partitionKey: "status", rowKey: processId.Replace(".jpeg", ""));

            // Modify the entity
            entityToUpdate.Value.Message = status;

            // Update the entity in the table
            await tableClient.UpdateEntityAsync<Status>(entityToUpdate.Value, ETag.All, TableUpdateMode.Replace);


        }

        /// <summary>
        /// Query to get the status of executing function
        /// </summary>
        /// <param name="processId">Process id for the process to be checked</param>
        /// <param name="tableName">Table name</param>
        /// <returns></returns>
        public Status? GetStatusOfProcess(string processId, string tableName = "ProcessStatus")
        {
            var connectionString = _configuration.GetConnectionString("SpeechifyCosmosDbConnectionString");
            var tableClient = new TableClient(connectionString, tableName);

            var process = tableClient.Query<Status>(status => status.RowKey == processId.Replace(".jpeg", "")).FirstOrDefault();

            return process;
        }
    }
}
