using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkCount
{
    class Program
    {
        public static string BaseAddress = @"http://instrument33.ru";
        public static int MaxLoopCount = 1;
        public static int LoopCount;
        public static StringComparer StringComparer = new StringComparer();

        static void Main(string[] args)
        {
            var allPaths = GetPathsInLoop(new List<string> {"/"}).ToList();

        }

        public static IEnumerable<string> GetPathsInLoop(IEnumerable<string> paths)
        {
            if (++LoopCount > MaxLoopCount)
            {
                return new List<string>();
            }
            
            var tasks = from path in paths
                        select Task.Run(async () => await GetPathsAsync(path));
            Task.WaitAll(tasks.ToArray());

            var allPaths = new List<string>();
            foreach (var task in tasks)
            {
                allPaths.AddRange(task.Result);
            }

            var newPaths = allPaths.Distinct(StringComparer).Except(paths, StringComparer);
            var getNewPaths = GetPathsInLoop(newPaths);

            return paths.Concat(newPaths).Concat(getNewPaths);
        }

        public static async Task<IEnumerable<string>> GetPathsAsync(string path)
        {
            var web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(BaseAddress + path);
            var aTags = htmlDoc.DocumentNode.Descendants("a").ToList();
            var listNewLinks = new List<string>();

            foreach (var aTag in aTags)
            {
                var hrefAttr = aTag.GetAttributeValue("href", null);

                if (!string.IsNullOrWhiteSpace(hrefAttr)
                    && hrefAttr.StartsWith('/'))
                {
                    listNewLinks.Add(hrefAttr);
                }
            }

            return listNewLinks.Distinct(StringComparer);
        }


    }
}
