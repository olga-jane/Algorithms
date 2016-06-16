using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestWay
{
    public class QuickPathDeepSearchException : ApplicationException
    {
        public QuickPathDeepSearchException(string message) : base(message) { }
    }

    public class QuickPathDeepSearch : QuickPathSearch
    {
        public QuickPathDeepSearch(bool isDiagonaleCrossing = false)
            : base(isDiagonaleCrossing)
        { }


        private struct PathItem
        {
            public Node node;
            public int nextIndex;
            public int nextMaxIndex;
        }

        private struct Path
        {
            public PathItem[] path;
            public int size;

            public decimal passTime;

            public int Index { get { return size - 1; } }

            public Node NextPotentialNode { get { return path[Index].node.linkedNodes[path[Index].nextIndex]; } }

            public bool IsNextPotentialNodeAlreadyInPath
            {
                get
                {
                    bool result = false;
                    for (int i = 0; i < size; i++)
                    {
                        if (path[i].node == NextPotentialNode)
                        {
                            result = true;
                            break;
                        }
                    }
                    return result;
                }
            }

            public void AddNode()
            {
                int oldIndex = Index;
                size++;

                path[Index].node = path[oldIndex].node.linkedNodes[path[oldIndex].nextIndex];
                path[Index].nextIndex = 0;
                path[Index].nextMaxIndex = path[Index].node.linkedNodes.Count - 1;

                passTime += path[Index].node.PassTime;
            }

            public void RemoveLastNode()
            {
                passTime -= path[Index].node.PassTime;
                size--;
            }

            public bool IsNewWaysLeft { get { return path[Index].nextMaxIndex >= path[Index].nextIndex; } }
            public bool IsNewWaysLeftFromStart { get { return size > 0 && path[0].nextMaxIndex >= path[0].nextIndex; } }

            public void ChooseNextDirection()
            {
                path[Index].nextIndex++;
            }

            public bool Reached(Node node)
            {
                return path[Index].node == node;
            }

            public void Assign(Path path)
            {
                for (int index = 0; index < path.size; index++)
                {
                    this.path[index] = path.path[index];
                }
                this.passTime = path.passTime;
                this.size = path.size;
            }

        }

        private void PrintPath(Path path, string message = null)
        {
            Console.WriteLine("\n\n");
            for (int index = 0; index < path.size; index++)
            {
                Console.Write("  [{0, 2},{1, 2}] {2, 2}", path.path[index].node.x, path.path[index].node.y, path.path[index].node.Speed);
            }
            Console.Write("  ----- PATH {1} ({0}) ({2}) -----^ ", path.passTime, message ?? "", "");
        }

        private const decimal NoPassTime = -1.0M;

        protected override Location[] FindQuickPathJob(Node startNode, Node endNode)
        {
            Path min = new Path { path = new PathItem[AllPassableNodes.Count()], size = 0, passTime = NoPassTime };

            Path current = new Path { path = new PathItem[AllPassableNodes.Count()], size = 1, passTime = 0M };
            current.path[0].node = startNode;
            current.path[0].nextIndex = 0;
            current.path[0].nextMaxIndex = startNode.linkedNodes.Count - 1;
            current.size = 1;

            while (current.IsNewWaysLeftFromStart)
            {
                // step forward while is able to
                while (!current.Reached(endNode) && current.IsNewWaysLeft)
                {
                    if (current.IsNextPotentialNodeAlreadyInPath || min.passTime != NoPassTime && current.passTime + current.NextPotentialNode.PassTime > min.passTime)
                    {
                        current.ChooseNextDirection();
                    }
                    else
                    {
                        current.AddNode();
                    }
                }

                if (current.Reached(endNode))
                {
                    if (min.passTime == NoPassTime || min.passTime > current.passTime)
                    {
                        min.Assign(current);
                    }
                }

                // step back if nowhere to go
                while (current.size > 1 && !current.IsNewWaysLeft || current.Reached(endNode))
                {
                    current.RemoveLastNode();
                }
                current.ChooseNextDirection();
            }

            Location[] result = new Location[min.size];
            for (int index = 0; index < min.size; index++)
            {
                result[index] = new Location(min.path[index].node.x, min.path[index].node.y);
            }

            return result;
        }
    }
}
