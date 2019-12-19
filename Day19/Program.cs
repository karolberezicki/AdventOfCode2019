using IntCode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day19
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");

            var memoryState = input.Split(",")
                .Select(long.Parse)
                .ToList();

            var part1 = Part1(memoryState);
            var part2 = Part2(memoryState);

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        private static bool GetBeamReading(IEnumerable<long> memoryState, int x, int y)
        {
            var icc = new IntCodeComputer(memoryState);
            icc.Inputs.Enqueue(x);
            icc.Inputs.Enqueue(y);
            icc.RunIntCode();
            return icc.Output.Dequeue() != 0;
        }

        private static int Part1(IReadOnlyCollection<long> memoryState)
        {
            var picture = new Dictionary<(int X, int Y), bool>();
            for (var x = 0; x < 50; x++)
            {
                for (var y = 0; y < 50; y++)
                {
                    picture[(x, y)] = GetBeamReading(memoryState, x, y);
                }
            }
            return picture.Values.Count(c => c);
        }

        private static int Part2(IReadOnlyCollection<long> memoryState)
        {
            var santaShip = (X: 0, Y: 1000);
            while (true)
            {
                while (!GetBeamReading(memoryState, santaShip.X, santaShip.Y))
                {
                    santaShip = (santaShip.X + 1, santaShip.Y);
                }

                if (GetBeamReading(memoryState, santaShip.X + 99, santaShip.Y - 99))
                {
                    santaShip = (santaShip.X, santaShip.Y - 99);
                    break;
                }

                santaShip = (santaShip.X, santaShip.Y + 1);
            }
            return 10000 * santaShip.X + santaShip.Y;
        }
    }
}
