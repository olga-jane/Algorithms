using ShortestWay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    static class LocationHelper
    {
        public static bool AreEqual(Location l1, Location l2)
        {
            return l1.X == l2.X && l1.Y == l2.Y;
        }
    }
}
