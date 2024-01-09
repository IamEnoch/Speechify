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

namespace SpeechifyFunctions.Interfaces
{
    public interface IAzureStorageService
    {
        /// <summary>
        /// CreateAzureTableAsync
        /// </summary>
        /// <returns>Name of the table</returns>
        public Task<String> CreateAzureTableAsync();

        /// <summary>
        /// Add an entity to the table
        /// </summary>
        /// <param name="processId">Process id created using the blob id</param>
        /// <param name="tableName">Table name</param>
        /// <param name="status">Status of the operation</param>
        /// <returns></returns>
        public Task AddEntityToTableAsync(string processId, string tableName, StatusEnum status);


        /// <summary>
        /// Update entity in the table
        /// </summary>
        /// <param name="processId">Process id created using the blob id</param>
        /// <param name="tableName">Table name</param>
        /// <param name="status">Status of the operation</param>
        /// <returns></returns>
        public Task UpdateEntityToTableAsync(string processId, string tableName, StatusEnum status);

        /// <summary>
        /// Query to get the status of executing function
        /// </summary>
        /// <param name="processId">Process id for the process to be checked</param>
        /// <param name="tableName">Table name</param>
        /// <returns></returns>
        public Status? GetStatusOfProcess(string processId, string tableName = "ProcessStatus");
    }
}
