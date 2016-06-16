using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestWay
{
    /// <summary>
    /// !!! Source code from task was not able to compile:
    /// "cells" obviously contains cyrillic letters and (byte) cast is missing.
    /// </summary>

    public class World
    {
        private Cell[,] cells; // Карта мира

        public string LastErrorMessage { get; private set; }

        public int Rows { get { return cells.GetLength(0); } }
        public int Cols { get { return cells.GetLength(1); } }

        public Cell this[int row, int col]
        {
            get
            {
                return cells[row,col];
            }
        }

        /// <summary>
        /// builds world map based on data provided by DataProviderFunc
        /// </summary>
        /// <param name="sizeX">size of 0 dimension of world map</param>
        /// <param name="sizeY">size of 1 dimension of world map</param>
        /// <param name="DataProviderFunc">should guarantee to provide data within 0 - sizeX-1 and 0 - sizeY-1</param>
        public World(int sizeX, int sizeY, Func<int, int, byte> DataProviderFunc)
        {
            cells = new Cell[sizeX, sizeY];
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Cols; y++)
                {
                    cells[x, y] = new Cell(x, y, DataProviderFunc(x, y));
                }
            }
        }

        /// <summary>
        /// builds world map based on random data
        /// </summary>
        /// <param name="sizeX">size of 0 dimension of world map</param>
        /// <param name="sizeY">size of 1 dimension of world map</param>
        public World(int sizeX, int sizeY)
        {
            var rnd = new Random();

            // Строим картy и рандомно ставим проходимость для каждой клетки.
            cells = new Cell[sizeX, sizeY];
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Cols; y++)
                {
                    cells[x, y] = new Cell(x, y, (byte)rnd.Next(0, 100));
                }
            }
        }

        /// <summary>
        /// returns fastest path between startLoc and endLoc.
        /// can return null if an error happened.
        /// </summary>
        /// <param name="startLoc"></param>
        /// <param name="endLoc"></param>
        /// <param name="strategy">search strategy</param>
        /// <returns>array of locations with fastest path. If unsuccessful, returns null.</returns>
        public Location[] FindShortestWay(Location startLoc, Location endLoc, ISquareQuickPathStrategy strategy)
        {
            Location[] result = null;

            try
            {
                if (strategy?.Setup(cells) ?? false)
                {
                    result = strategy?.FindQuickPath(startLoc, endLoc)?.ToArray() ?? null;
                }
                LastErrorMessage = strategy == null ? "No strategy" : "";
            }
            catch (Exception e)
            {
                LastErrorMessage = e.Message + ", " + e.InnerException?.Message ?? "";
            }
            return result;
        }
    }
}
