using IntCode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day13
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
            Console.WriteLine($"Part1 {part1}");
        }

        private static int Part1(IEnumerable<long> memoryState)
        {
            var icc = new IntCodeComputer(memoryState);
            icc.RunTillHalt();
            var screenOutput = icc.Output;
            var tiles = GetTiles(screenOutput);
            Display(tiles);
            return tiles.Count(t => t.Id == 2);
        }

        private static HashSet<(long X, long Y, long Id)> GetTiles(List<long> screenOutput)
        {
            var tiles = new HashSet<(long X, long Y, long Id)>();

            for (var i = 0; i < screenOutput.Count; i += 3)
            {
                var x = screenOutput[i];
                var y = screenOutput[i + 1];
                var tileId = screenOutput[i + 2];
                tiles.Add((x, y, tileId));
            }

            return tiles;
        }

        public static void Display(HashSet<(long X, long Y, long Id)> tiles)
        {
            Console.Clear();

            for (var y = 0; y < 20; y++)
            {
                for (var x = 0; x < 44; x++)
                {
                    var (_, _, id) = tiles.FirstOrDefault(t => t.X == x && t.Y == y);
                    switch (id)
                    {
                        case 1:
                            Console.Write("█");
                            break;
                        case 2:
                            Console.Write("░");
                            break;
                        case 3:
                            Console.Write("═");
                            break;
                        case 4:
                            Console.Write("o");
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }
                }
                Console.WriteLine();
            }

            var (_, _, score) = tiles.FirstOrDefault(t => t.X == -1 && t.Y == 0);
            Console.WriteLine($"Score: {score:D6}");
        }
    }
}
