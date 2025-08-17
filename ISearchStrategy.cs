using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public interface ISearchStrategy
    {
        List<MenuItem> Search(List<MenuItem> items, SearchCriteria criteria);
    }
}
