using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TreeTraversalPerformance
{
    public class Tree : ParentNode
    {
    }

    [DebuggerDisplay("Count = {Count}, CCount = {Children.Count}")]
    public abstract class ParentNode
    {
        public List<Node> Children { get; set; }
        public int Count;

        protected ParentNode()
        {
            Children = new List<Node>();
        }

        public int SetCount()
        {
            Count = Children.Sum(node => node.SetCount()) + Children.Count;
            return Count;
        }

        public delegate bool TraverseFunc(ParentNode pn, Node n);

        public void TraversePairwise(TraverseFunc func)
        {
            var funcContinue = true;
            var dirs = new Stack<ParentNode>(10);
            dirs.Push(this);

            while (funcContinue && dirs.Count > 0)
            {
                var pn = dirs.Pop();

                if (pn.Children != null)
                {
                    foreach (var n in pn.Children)
                    {
                        if (func != null)
                        {
                            funcContinue = func(pn, n);
                            if (!funcContinue)
                            {
                                break;
                            }
                        }

                        if (n.Children.Count > 0)
                        {
                            dirs.Push(n);
                        }
                    }
                }
            }
        }
    }

    [DebuggerDisplay("Name = {Name}, Count = {Count}, CCount = {Children.Count}")]
    public class Node : ParentNode
    {
        public string Name { get; set; }
    }
}