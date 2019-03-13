using System.Linq;

namespace LinkCount
{
    class Program
    {
        public static string BaseAddress = @"http://instrument33.ru";
        public static int MaxDepth = 1;

        static void Main(string[] args)
        { 
            var counter = new LinkCounter(BaseAddress, MaxDepth);
            var allPaths = counter.GetPathsInLoop().ToList();
        }
    }
}
