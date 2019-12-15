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
            Console.CursorVisible = false;

            var input = System.IO.File.ReadAllText("input.txt");

            var memoryState = input.Split(",")
                .Select(long.Parse)
                .ToList();

            var part1 = Part1(memoryState);
            var part2 = Part2(memoryState);

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        private static int Part1(IEnumerable<long> memoryState)
        {
            var icc = new IntCodeComputer(memoryState);
            icc.RunTillHalt();
            var screenOutput = icc.Output.ToList();
            Display(screenOutput);
            var tiles = new Dictionary<(long X, long Y), long>();
            UpdateTiles(tiles, screenOutput);
            return tiles.Count(t => t.Value == 2);
        }

        private static long Part2(IEnumerable<long> memoryState)
        {

            var icc = new IntCodeComputer(memoryState);
            icc.SetAddress(0, 2);

            var tiles = new Dictionary<(long X, long Y), long>();

            Console.SetCursorPosition(0, 20);
            Console.WriteLine(new string('█', 44));

            while (!icc.IsHalted)
            {
                icc.RunIntCode(BreakMode.Input);
                var screenOutput = icc.Output.ToList();
                Display(screenOutput);
                UpdateTiles(tiles, screenOutput);
                icc.Output.Clear();

                var ball = tiles.First(t => t.Value == 4).Key;
                var paddle = tiles.First(t => t.Value == 3).Key;

                icc.Inputs.Enqueue(Math.Sign(ball.X - paddle.X));
            }

            return tiles[(-1, 0)];
        }

        private static void Display(IReadOnlyList<long> screenOutput)
        {
            for (var i = 0; i < screenOutput.Count; i += 3)
            {
                var x = (int)screenOutput[i];
                var y = (int)screenOutput[i + 1];
                var tileId = screenOutput[i + 2];

                if (x == -1)
                {
                    Console.SetCursorPosition(0, 21);
                    Console.WriteLine($"Score: {tileId:D5}");
                    continue;
                }

                Console.SetCursorPosition(x, y);
                switch (tileId)
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
        }

        private static void UpdateTiles(Dictionary<(long X, long Y), long> tiles, IReadOnlyList<long> screenOutput)
        {
            for (var i = 0; i < screenOutput.Count; i += 3)
            {
                var x = screenOutput[i];
                var y = screenOutput[i + 1];
                var tileId = screenOutput[i + 2];
                tiles[(x, y)] = tileId;
            }
        }
    }
}
