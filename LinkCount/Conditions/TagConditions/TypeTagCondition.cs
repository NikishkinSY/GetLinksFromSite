using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace LinkCount.Conditions.TagConditions
{
    public class TypeTagCondition: ITagCondition
    {
        private string _tagType { get; }
        private string _attributeType { get; }

        public TypeTagCondition(string tagType = "a", string attributeType = "href")
        {
            _tagType = tagType;
            _attributeType = attributeType;
        }

        public IEnumerable<string> Filter(HtmlNode parentNode)
        {
            return from node in parentNode.Descendants(_tagType)
                   select node.GetAttributeValue(_attributeType, null);
        }
    }
}
