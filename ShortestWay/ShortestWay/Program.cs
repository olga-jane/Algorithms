using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestWay
{
    class Program
    {
        static void Main(string[] args)
        {
            World world = new World(6, 8);
            Location from = new Location(1, 1);
            Location to = new Location(5, 4);

            Console.WriteLine("Searching shortest way for the map from {0} to {1}, with cells passability:", from, to);

            for(int row = 0; row < world.Rows; row++)
            {
                for (int col = 0; col < world.Cols; col++)
                {
                    Console.Write("{0, 4}", world[row, col].Passability);
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            Location[] loc = world.FindShortestWay(
                from, to, new QuickPathDeepSearch(isDiagonaleCrossing: true));

            if (loc != null)
            {
                for (int index = 0; index < loc.Length; index++)
                {
                    Console.Write("[{0},{1}] ", loc[index].X, loc[index].Y);
                }
            }
            else
            {
                Console.WriteLine(world.LastErrorMessage);
            }
            Console.WriteLine("\n\nDone. Press any key...");
            Console.ReadKey(true);
            
        }
    }
}
