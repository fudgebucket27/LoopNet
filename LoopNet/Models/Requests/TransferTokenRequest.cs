using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Requests
{
    /// <summary>
    /// Contains transfer token request information
    /// </summary>
    public class TransferTokenRequest
    {
        /// <summary>
        /// The exchange address
        /// </summary>
        public string? Exchange { get; set; }
        /// <summary>
        /// The payer account id
        /// </summary>
        public int PayerId { get; set; }
        /// <summary>
        /// The payer hex address
        /// </summary>
        public string? PayerAddr { get; set; }
        /// <summary>
        /// The payee account id
        /// </summary>
        public int PayeeId { get; set; } = 0;           // Default of 0 if unknown is fine
        /// <summary>
        /// The payee hex address
        /// </summary>
        public string? PayeeAddr { get; set; }
        /// <summary>
        /// The token to transfer
        /// </summary>
        public Token? Token { get; set; }
        /// <summary>
        /// The fee token
        /// </summary>
        public Token? MaxFee { get; set; }
        /// <summary>
        /// The storage id
        /// </summary>
        public int StorageId { get; set; }
        /// <summary>
        /// The unix timestamp for the transfer expiry
        /// </summary>
        public long ValidUntil { get; set; }
        /// <summary>
        /// The token name
        /// </summary>
        public string? TokenName { get; set; }
        /// <summary>
        /// The token fee name
        /// </summary>
        public string? TokenFeeName { get; set; }
    }
}
