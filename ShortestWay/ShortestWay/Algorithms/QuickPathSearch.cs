using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestWay
{
    public class QuickPathSearchException : ApplicationException
    {
        public QuickPathSearchException(string message) : base(message) { }
    }

    public abstract class QuickPathSearch : ISquareQuickPathStrategy
    {
        protected class Node
        {
            public readonly int x;
            public readonly int y;
            public readonly IList<Node> linkedNodes = new List<Node>();
            public Node(int x, int y, byte speed)
            {
                this.x = x;
                this.y = y;
                this.speed = speed;
            }
            public decimal PassTime { get { return timeTable[speed]; } }
            public string Speed {  get { return speed.ToString(); } }

            private readonly byte speed;

            #region --- Static data ---
            // TODO: safety in multithread environment
            /// <summary>
            /// passing time precalculation
            /// </summary>
            private static decimal[] timeTable = new decimal[byte.MaxValue];

            static Node()
            {
                timeTable[0] = 1000000; // big enough to compare with
                for (int index = 1; index < timeTable.Length; index++)
                {
                    timeTable[index] = 1.0M / index;
                }
            }
            #endregion --- Static data ---
        }

        public IEnumerable<Location> FindQuickPath(Location start, Location end)
        {
            if (!IsSetup)
            {
                throw new QuickPathSearchException("Please setup data first");
            }
            Node startNode = AllPassableNodes.FirstOrDefault(n => n.x == start.X && n.y == start.Y);
            Node endNode = AllPassableNodes.FirstOrDefault(n => n.x == end.X && n.y == end.Y);

            if (startNode == null || endNode == null)
            {
                throw new QuickPathSearchException((startNode == null ? "Start" : "End") + " location does not exist or is unpassable");
            }

            Location[] result = new Location[0];
            if (start.X == end.X && start.Y == end.Y)
            {
                result = new Location[2] { new Location(start), new Location(end) };
            }
            else
            {
                result = FindQuickPathJob(startNode, endNode);
            }
            return result;
        }

        protected abstract Location[] FindQuickPathJob(Node startNode, Node endNode);

        public bool IsSetup { get; private set; } = false;
        private readonly List<Node> allPassableNodes = new List<Node>();

        private bool isDiagonaleCrossing;

        public QuickPathSearch(bool isDiagonaleCrossing)
        {
            this.isDiagonaleCrossing = isDiagonaleCrossing;
        }

        protected IEnumerable<Node> AllPassableNodes { get { return allPassableNodes; } }

        private void TryAdd(Node holder, List<Node> list, int x, int y)
        {
            Node found = list.Find(n => n.x == x && n.y == y);
            if (found != null)
            {
                holder.linkedNodes.Add(found);
            }
        }

        /// <summary>
        /// This method does not use location coordinates inside cell. If Cell is going to have another x and y that it's coordinates in array,
        /// method should be redesigned
        /// </summary>
        /// <param name="field"></param>
        /// <returns>success of creating a pre-setup graph for algorithm</returns>
        public bool Setup(Cell[,] field)
        {
            IsSetup = false;
            if (field.GetLength(0) > 0 && field.GetLength(1) > 0)
            {
                allPassableNodes.Clear();

                for (int x = 0; x < field.GetLength(0); x++)
                {
                    for (int y = 0; y < field.GetLength(1); y++)
                    {
                        if (field[x, y].Passability != Cell.NoPassability)
                        {
                            allPassableNodes.Add(new Node(x, y, field[x, y].Passability));
                        }
                    }
                }
                foreach (var node in allPassableNodes)
                {
                    TryAdd(node, allPassableNodes, node.x - 1, node.y);
                    TryAdd(node, allPassableNodes, node.x, node.y - 1);
                    TryAdd(node, allPassableNodes, node.x + 1, node.y);
                    TryAdd(node, allPassableNodes, node.x, node.y + 1);

                    if (isDiagonaleCrossing)
                    {
                        TryAdd(node, allPassableNodes, node.x - 1, node.y - 1);
                        TryAdd(node, allPassableNodes, node.x + 1, node.y - 1);
                        TryAdd(node, allPassableNodes, node.x + 1, node.y + 1);
                        TryAdd(node, allPassableNodes, node.x - 1, node.y + 1);
                    }
                }
                IsSetup = true;
            }
            return IsSetup;
        }
    }
}
