using Azure;
using Azure.Data.Tables;
using SpeechifyFunctions.Enums;

namespace SpeechifyFunctions.Models
{
    public record Status : ITableEntity
    {
        public string RowKey { get; set; } = default!;
        public string PartitionKey { get; set; } = "status";
        public DateTimeOffset? Timestamp { get; set; }
        public StatusEnum Message { get; set; }
        public ETag ETag { get; set; }

    }
}