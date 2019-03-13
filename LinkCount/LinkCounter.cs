using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkCount.Conditions;

namespace LinkCount
{
    public class LinkCounter
    {
        private readonly string _baseAddress;
        private readonly int _maxDepth;
        private readonly StringComparer _stringComparer;
        private int _loopCount;
        private IEnumerable<ICondition> _conditions = new List<ICondition>();

        public LinkCounter(string baseAddress, int maxDepth = 1)
        {
            _baseAddress = baseAddress;
            _maxDepth = maxDepth;
            _stringComparer = new StringComparer();
        }

        public IEnumerable<string> GetPathsInLoop(IEnumerable<string> paths = null)
        {
            if (string.IsNullOrWhiteSpace(_baseAddress))
                return new List<string>();

            if (++_loopCount > _maxDepth)
                return new List<string>();

            if (paths == null)
                paths = new List<string> { "/" };

            var tasks = from path in paths
                select Task.Run(async () => await GetPathsAsync(path));
            Task.WaitAll(tasks.ToArray());
            
            var allPaths = tasks.SelectMany(x => x.Result);
            var newPaths = allPaths.Distinct(_stringComparer).Except(paths, _stringComparer);
            var getNewPaths = GetPathsInLoop(newPaths);

            return paths.Concat(newPaths).Concat(getNewPaths);
        }

        private async Task<IEnumerable<string>> GetPathsAsync(string path)
        {
            var web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(_baseAddress + path);
            var aTags = htmlDoc.DocumentNode.Descendants("a").ToList();

            var listNewLinks = from aTag in aTags
                let hrefAttr = aTag.GetAttributeValue("href", null)
                where hrefAttr?.StartsWith('/') ?? false
                select hrefAttr;

            return listNewLinks.Distinct(_stringComparer);
        }
    }
}
