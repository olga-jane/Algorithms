using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestWay
{

    /// <summary>
    /// If Cell is going to have another x and y that it's coordinates in array,
    /// usage of Cell should be redesigned
    /// </summary>
    public class Cell : Location
    {
        // Определяет проходимость клетки от 0(невозможно пройти) до 100(обычная проходимость).
        // Чем выше проходимость, тем быстрее можно пересечь клетку.

        public byte Passability { get; private set; }
        public const int MaxPassability = byte.MaxValue;
        public const int NoPassability = 0;

        public Cell(int x, int y, byte passability)
            : base(x, y)
        {
            this.Passability = passability;
        }
    }
}
