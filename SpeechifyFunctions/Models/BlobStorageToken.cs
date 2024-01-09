using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechifyFunctions.Models
{
    public class BlobStorageToken
    {
        public DateTime ExpiresOn { get; set; }
        public string Token { get; set; }
    }
}
