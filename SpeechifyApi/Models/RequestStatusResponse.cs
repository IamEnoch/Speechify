namespace SpeechifyApi.Models
{
    public class RequestStatusResponse
    {
        public Value Value { get; set; }
        public List<object> Formatters { get; set; }
        public List<object> ContentTypes { get; set; }
        public object? DeclaredType { get; set; }
        public int StatusCode { get; set; }
    }

    
}
