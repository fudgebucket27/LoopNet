using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Requests
{
    public class TransferTokenRequest
    {
        public string? Exchange { get; set; }
        public int PayerId { get; set; }
        public string? PayerAddr { get; set; }
        public int PayeeId { get; set; } = 0;           // Default of 0 if unknown is fine
        public string? PayeeAddr { get; set; }
        public Token? Token { get; set; }
        public Token? MaxFee { get; set; }
        public int StorageId { get; set; }
        public long ValidUntil { get; set; }
        public string? TokenName { get; set; }
        public string? TokenFeeName { get; set; }
    }
}
