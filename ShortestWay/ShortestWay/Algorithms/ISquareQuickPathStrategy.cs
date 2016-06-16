using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestWay
{
    public interface ISquareQuickPathStrategy
    {
        bool Setup(Cell[,] field);
        IEnumerable<Location> FindQuickPath(Location start, Location end);
    }
}
