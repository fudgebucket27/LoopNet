using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models
{
    public class Market
    {
        public string? market { get; set; }
        public int baseTokenId { get; set; }
        public int quoteTokenId { get; set; }
        public int precisionForPrice { get; set; }
        public int orderbookAggLevels { get; set; }
        public bool enabled { get; set; }
    }

    public class Markets
    {
        public List<Market>? markets { get; set; }
    }
}
