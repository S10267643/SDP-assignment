using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public sealed class SearchService
    {
        private ISearchStrategy _strategy;

        public void SetStrategy(ISearchStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public List<MenuItem> Search(List<MenuItem> items, SearchCriteria criteria)
        {
            if (_strategy == null) throw new InvalidOperationException("Search strategy not set.");
            return _strategy.Search(items, criteria);
        }
    }
}
