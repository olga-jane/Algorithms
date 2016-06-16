using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestWay
{
    public class Location
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Location(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Location(Location l)
        {
            this.X = l.X;
            this.Y = l.Y;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", X, Y);
        }
    }

}
