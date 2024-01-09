namespace SpeechifyApi.Models
{
    public class BlobStorageToken
    {
        public DateTime ExpiresOn { get; set; }
        public string Token { get; set; }
    }
}