using IntCode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day11
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");

            var memoryState = input.Split(",")
                .Select(long.Parse)
                .ToList();

            var paintedPanels = PaintHull(memoryState);

            var part1 = paintedPanels.Select(p => (p.X, p.Y)).Distinct().Count();

            Console.WriteLine($"Part1 {part1}");

            var paintedPanels2 = PaintHull(memoryState, true);

            var pixels = new HashSet<(int X, int Y)>(
                paintedPanels2
                    .Skip(1)
                    .Take(paintedPanels2.Count - 2)
                    .Where(p => p.Color == 1)
                    .Select(p => (p.X, -p.Y)));

            var maxX = pixels.Select(p => p.X).Max();
            var maxY = pixels.Select(p => p.Y).Max();

            Console.WriteLine("Part2");
            foreach (var y in Enumerable.Range(0, maxY + 1))
            {
                var line = string.Join("", Enumerable.Range(0, maxX + 1)
                    .Select(x => pixels.Contains((x, y)) ? '█' : ' '));
                Console.WriteLine(line);
            }
        }

        private static List<(int X, int Y, int Color)> PaintHull(IEnumerable<long> memoryState, bool startOnWhite = false)
        {
            var paintedPanels = new List<(int X, int Y, int Color)>();
            var currentDirection = Direction.Up;
            var currentPosition = (X: 0, Y: 0);
            var icc = new IntCodeComputer(memoryState);

            if (startOnWhite)
            {
                paintedPanels.Add((currentPosition.X, currentPosition.Y, 1));
            }

            while (!icc.IsHalted)
            {
                var currentPanelColor = paintedPanels
                    .LastOrDefault(p => p.X == currentPosition.X && p.Y == currentPosition.Y).Color;

                icc.Inputs.Add(currentPanelColor);

                icc.RunIntCode();
                var colorToPaint = (int)icc.Output.Last();
                icc.RunIntCode();
                var turnDirection = (int)icc.Output.Last();

                paintedPanels.Add((currentPosition.X, currentPosition.Y, colorToPaint));
                currentDirection = currentDirection.Turn(turnDirection);
                currentPosition = currentDirection.Move(currentPosition);
            }

            return paintedPanels;
        }
    }
}
