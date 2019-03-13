using System;
using System.Collections.Generic;
using System.Linq;
using LinkCount.Conditions;
using LinkCount.Conditions.LinkConditions;
using LinkCount.Conditions.PageConditions;
using LinkCount.Conditions.TagConditions;

namespace LinkCount
{
    class Program
    {
        public static string BaseAddress = @"http://instrument33.ru";
        public static int MaxDepth = 1;

        static void Main(string[] args)
        {
            var counter = new LinkCounter(
                BaseAddress, 
                MaxDepth,
                new List<IPageCondition> { new SizePageCondition(200000) },
                new List<ITagCondition> { new TypeTagCondition("a", "href"), new TypeTagCondition("img", "src") },
                new List<ILinkCondition> { new ContainsLinkCondition("catalog") }
                );
            var allPaths = counter.GetPathsInLoop().ToList();

            Console.WriteLine($"For site={BaseAddress} with depth={MaxDepth} amount of links are: {allPaths.Count}");
            Console.ReadKey();
        }
    }
}
