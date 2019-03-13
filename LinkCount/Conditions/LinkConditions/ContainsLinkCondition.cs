using System.Collections.Generic;
using System.Linq;

namespace LinkCount.Conditions.LinkConditions
{
    public class ContainsLinkCondition: ILinkCondition
    {
        private string _contains { get; }

        public ContainsLinkCondition(string contains)
        {
            _contains = contains;
        }

        public IEnumerable<string> Filter(IEnumerable<string> links)
        {
            return from link in links
                   where link.Contains(_contains)
                   select link;
        }
    }
}
