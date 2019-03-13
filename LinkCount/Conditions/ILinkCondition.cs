using System.Collections.Generic;

namespace LinkCount.Conditions
{
    public interface ILinkCondition
    {
        IEnumerable<string> Filter(IEnumerable<string> links);
    }
}
