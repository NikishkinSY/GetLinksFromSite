﻿using HtmlAgilityPack;
using LinkCount.Conditions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkCount
{
    public class LinkCounter
    {
        private readonly string _baseAddress;
        private readonly int _maxDepth;
        private readonly StringComparer _stringComparer;
        private int _loopCount;
        private readonly List<IPageCondition> _pageConditions = new List<IPageCondition>();
        private readonly List<ITagCondition> _tagConditions = new List<ITagCondition>();
        private readonly List<ILinkCondition> _linkConditions = new List<ILinkCondition>();

        public LinkCounter(
            string baseAddress, 
            int maxDepth, 
            IEnumerable<IPageCondition> pageConditions,
            IEnumerable<ITagCondition> tagConditions,
            IEnumerable<ILinkCondition> linkConditions)
        {
            _baseAddress = baseAddress;
            _maxDepth = maxDepth;
            _stringComparer = new StringComparer();

            if (pageConditions != null)
                _pageConditions.AddRange(pageConditions);

            if (tagConditions != null)
                _tagConditions.AddRange(tagConditions);

            if (linkConditions != null)
                _linkConditions.AddRange(linkConditions);
        }

        public IEnumerable<string> GetPathsInLoop(IEnumerable<string> paths = null)
        {
            if (string.IsNullOrWhiteSpace(_baseAddress))
                return new List<string>();

            if (++_loopCount > _maxDepth)
                return new List<string>();

            if (paths == null)
                paths = new List<string> { "/" };
            
            // get pages in tasks
            var tasks = (from path in paths
                        where path?.StartsWith('/') ?? false
                        select Task.Run(async () => await GetPathsAsync(path))).ToList();
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

            // page conditions
            if (!_pageConditions.All(x => x.Check(htmlDoc.DocumentNode)))
                return new List<string>();

            // tag conditions
            var links = _tagConditions.Select(x => x.Filter(htmlDoc.DocumentNode)).SelectMany(x => x);

            // link conditions
            var filteredLinks = _linkConditions.Any()
                ? _linkConditions.Select(x => x.Filter(links)).SelectMany(x => x)
                : links;

            return filteredLinks.Distinct(_stringComparer);
        }
    }
}
