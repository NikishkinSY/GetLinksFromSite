using HtmlAgilityPack;

namespace LinkCount.Conditions
{
    public interface IPageCondition
    {
        bool Check(HtmlNode node);
    }
}
