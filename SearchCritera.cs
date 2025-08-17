using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public sealed class SearchCriteria
    {
        public string Query { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal? MaxPrice { get; set; }
    }
}
