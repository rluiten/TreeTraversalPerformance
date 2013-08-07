
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace TreeTraversalPerformance
{
    class Program
    {
        static void Main()
        {
            var t0 = BuildTestTree(11345,    2000, 8);
            DoTests(t0, "t0");
            t0 = null;

            var t1 = BuildTestTree(43534,   20000, 8);
            DoTests(t1, "t1");
            t1 = null;

            var t2 = BuildTestTree(73453,  200000, 10);
            DoTests(t2, "t2");
            t2 = null;

            var t3 = BuildTestTree(92323, 2000000, 12);
            DoTests(t3, "t3");
            t3 = null;

            //var t4 = BuildTestTree(23423, 4000000, 10);
            //DoTests(t4, "t4");
            //t4 = null;

            //if (Debugger.IsAttached)
            //{
            //    Console.WriteLine("Press enter to exit...");
            //    Console.ReadLine();
            //}
        }

        public static void DoTests(Tree t, string testId)
        {
            Console.WriteLine("{0} Node Count {1}", testId, t.Count);
            Console.WriteLine("{0} largest child count {1}", testId, LargestChildCount(t));
            var repeat = 40;
            if (testId == "t0")
            {
                repeat = 400;
            }

            GC.Collect();
            System.Threading.Thread.Sleep(500);
            var traverseMsec = DoTraverseListTest(t, repeat);
            var avgTraverse = (double) traverseMsec/repeat;
            Console.WriteLine(" DoTraverseListTest totalTime {0}", traverseMsec);
            Console.WriteLine(" DoTraverseListTest perRun {0}", avgTraverse);
            Console.WriteLine(" DoTraverseListTest perNode {0}", avgTraverse / t.Count);

            GC.Collect();
            System.Threading.Thread.Sleep(500);
            repeat = repeat / 4;
            var enumMsec = DoEnumeratorListTest(t, repeat);
            var avgEnumerator = (double)enumMsec / repeat;
            Console.WriteLine(" DoEnumeratorListTest totalTime {0}", enumMsec);
            Console.WriteLine(" DoEnumeratorListTest perRun {0}", avgEnumerator);
            Console.WriteLine(" DoTraverseListTest perNode {0}", avgEnumerator / t.Count);

            Console.WriteLine(" Enumerator / Traverse {0}", (double)avgEnumerator / avgTraverse);
            Console.WriteLine("");
        }

        public static long DoTraverseListTest(ParentNode parentNode, int repeatCount)
        {
            var sw = new Stopwatch();
            sw.Start();
            var testlist = new List<ParentNode>();
            parentNode.TraversePairwise((p, d) => {
                testlist.Add(d);
                return true;
            });
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static long DoEnumeratorListTest(Tree tree, int repeatCount)
        {
            var sw = new Stopwatch();
            sw.Start();
            var nodeEnumerator = new NodeEnumerator(tree);
            for (var i = 0; i < repeatCount; i++)
            {
                var list = nodeEnumerator.ToList();
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private static Tree TestBuild()
        {
            // test tree
            var t = new Tree();
            AddPath(t, new[] { 2 });
            AddPath(t, new[] { 1, 2, 3 });
            AddPath(t, new[] { 1, 2, 4 });
            AddPath(t, new[] { 1, 2, 4, 6 });
            AddPath(t, new[] { 1, 2, 4, 7 });
            AddPath(t, new[] { 1, 2, 4, 8 });
            AddPath(t, new[] { 1, 2, 4, 9 });
            AddPath(t, new[] { 1, 3 });
            AddPath(t, new[] { 1, 1 });
            t.SetCount();
            DumpTree(t);
            return t;
        }

        private static Tree BuildTestTree(int seed, int pathCount, int maxDepth)
        {   // generate a random tree
            var r = new Random(seed);
            var t = new Tree();

            for (var i = 0; i < pathCount; ++i)
            {
                var newPath = new List<int>(maxDepth);
                var newDepth = r.Next(1, maxDepth);
                var maxEntriesAtLevel = 20;
                for (var j = 0; j < newDepth; ++j)
                {
                    var b = r.Next(0, maxEntriesAtLevel);
                    newPath.Add(b);
                    maxEntriesAtLevel *= 4;
                }
                //Console.WriteLine("{0}", string.Join(",", newPath.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray()));
                AddPath(t, newPath);
            }
            t.SetCount();
            //DumpTree(t);
            return t;
        }

        private static void AddPath(Tree t, IEnumerable<int> newPath)
        {
            ParentNode parentNode = t;
            foreach (var pathElement in newPath)
            {
                var element = pathElement;
                var findNode = parentNode.Children.FirstOrDefault(n => n.Name == element.ToString(CultureInfo.InvariantCulture));
                if (findNode == null)
                {
                    var newNode = new Node { Name = string.Format("{0}", pathElement) };
                    parentNode.Children.Add(newNode);
                    findNode = newNode;
                }
                parentNode = findNode;
            }
        }

        private static void DumpTree(ParentNode tree, int indent = 0)
        {
            foreach (var n in tree.Children)
            {
                Console.WriteLine("{0}{1} [{2}] {{{3}}}", "".PadLeft(indent, ' '), n.Name, n.Count, n.Children.Count);
                DumpTree(n, indent + 1);
            }
        }

        private static int LargestChildCount(ParentNode tree, int largestCountSoFar = -1)
        {
            var count = Math.Max(largestCountSoFar, tree.Children.Count);
            foreach (var n in tree.Children)
            {
                count = LargestChildCount(n, count);
            }
            return count;
        }
    }
}
