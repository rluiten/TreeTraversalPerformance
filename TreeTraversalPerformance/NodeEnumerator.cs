using System.Collections;
using System.Collections.Generic;

namespace TreeTraversalPerformance
{
    public class NodeEnumerator : IEnumerator<Node>, IEnumerable<Node>
    {
        private readonly IEnumerable<Tree> _trees;
        private Node _current;
        private Stack<ParentNode> _nodes;
        private int _childIndex;
        private ParentNode _currentParent;
        private const int NoIndex = -2;
        private const int StartingIndex = -1;

        public Node Current
        {
            get { return _current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public NodeEnumerator(Tree rootEntry)
        {
            _trees = new List<Tree> { rootEntry };
            Reset();
        }

        public NodeEnumerator(IEnumerable<Tree> trees)
        {
            _trees = trees;
            Reset();
        }

        private static Stack<ParentNode> StackOfRoots(IEnumerable<Tree> trees)
        {
            var parentNodes = new Stack<ParentNode>(10);
            foreach (var re in trees)
            {
                if (re.Children != null && re.Children.Count > 0)
                {
                    parentNodes.Push(re);
                }
            }
            return parentNodes;
        }

        public void Dispose()
        {
            _current = null;
            _nodes = null;
        }

        public bool MoveNext()
        {
            _current = null;
            if (_childIndex == NoIndex)
            {
                if (_nodes.Count > 0)
                {
                    _currentParent = _nodes.Pop();
                    _childIndex = StartingIndex;
                }
            }

            if (_childIndex != NoIndex)
            {
                _childIndex++;
                if (_childIndex < _currentParent.Children.Count)
                {
                    _current = _currentParent.Children[_childIndex];
                    if (_current.Children != null && _current.Children.Count > 0)
                    {
                        _nodes.Push(_current);
                    }
                }
                else
                {
                    _childIndex = NoIndex;
                    MoveNext();
                }
            }

            return _current != null;
        }

        public void Reset()
        {
            _current = null;
            _nodes = StackOfRoots(_trees);
            _childIndex = NoIndex;
        }

        IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
        {
            return new NodeEnumerator(_trees);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new NodeEnumerator(_trees);
        }
    }
}