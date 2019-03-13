using System.Collections.Generic;
using HtmlAgilityPack;

namespace LinkCount.Conditions
{
    public interface ITagCondition
    {
        IEnumerable<string> Filter(HtmlNode parentNode);
    }
}
