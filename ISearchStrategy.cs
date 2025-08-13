using System.Collections.Generic;

namespace SDP_assignment
{
    public interface ISearchStrategy
    {
        List<MenuItem> Search(List<MenuItem> items, SearchCriteria criteria);
    }
}
