using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Requests
{
    /// <summary>
    /// Red packet nft target
    /// </summary>
    public class RedPacketNftTarget
    {
        /// <summary>
        /// List of address to send the red packet nft to
        /// </summary>
        [JsonProperty("claimer")]
        public List<string>? Claimer { get; set; }
        /// <summary>
        /// The hash of the red packet nft
        /// </summary>
        [JsonProperty("hash")]
        public string? Hash { get; set; }
        /// <summary>
        /// Notification type, 0 for badge, 1 for push
        /// </summary>
        [JsonProperty("notifyType")]
        public int NotifyType { get; set; }
    }
}
