﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class TickersResponse
    {
        [JsonProperty("tickers")]

        public List<List<string>>? Tickers { get; set; }
    }
}