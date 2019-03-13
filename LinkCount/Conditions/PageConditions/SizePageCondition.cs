using HtmlAgilityPack;

namespace LinkCount.Conditions.PageConditions
{
    public class SizePageCondition: IPageCondition
    {
        private int _maxSymbols { get; }

        public SizePageCondition(int maxSymbol)
        {
            _maxSymbols = maxSymbol;
        }

        public bool Check(HtmlNode node)
        {
            return _maxSymbols >= node.InnerHtml.Length;
        }
    }
}
