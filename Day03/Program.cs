using System;
using System.Collections.Generic;
using System.Linq;

namespace Day03
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllLines("input.txt")
                .Select(l => l.Trim().Split(",").ToList())
                .ToList();

            var wire1 = GetPointsForWire(input[0]);
            var wire2 = GetPointsForWire(input[1]);

            var intersections = new HashSet<(int X, int Y)>(wire1.Select(p => (p.X, p.Y)).Intersect(wire2.Select(p => (p.X, p.Y))));
            
            var closestIntersection = intersections.Min(p => Math.Abs(p.X) + Math.Abs(p.Y));

            var fewestStepsIntersection = intersections
                .Min(intersection => wire1.First(p => p.X == intersection.X && p.Y == intersection.Y).Step +
                                     wire2.First(p => p.X == intersection.X && p.Y == intersection.Y).Step);

            Console.WriteLine($"Part1 {closestIntersection}");
            Console.WriteLine($"Part2 {fewestStepsIntersection}");
        }

        private static HashSet<(int X, int Y, int Step)> GetPointsForWire(List<string> instructions)
        {
            var currentPoint = (X : 0, Y: 0, Step: 0);
            var wire = new HashSet<(int X, int Y, int Step)>();

            foreach (var instruction in instructions)
            {
                var direction = instruction[0];
                var distance = int.Parse(string.Join("", instruction.Skip(1)));

                var move = new List<(int X, int Y, int Step)>();

                switch (direction)
                {
                    case 'R':
                        move = Enumerable.Range(1, distance)
                            .Select(p => (X : currentPoint.X + p, currentPoint.Y, Step : currentPoint.Step + p))
                            .ToList();
                        break;
                    case 'L':
                        move = Enumerable.Range(1, distance)
                            .Select(p => (X: currentPoint.X - p, currentPoint.Y, Step: currentPoint.Step + p))
                            .ToList();
                        break;
                    case 'U':
                        move = Enumerable.Range(1, distance)
                            .Select(p => (currentPoint.X, Y: currentPoint.Y + p, Step: currentPoint.Step + p))
                            .ToList();
                        break;
                    case 'D':
                        move = Enumerable.Range(1, distance)
                            .Select(p => (currentPoint.X, Y: currentPoint.Y - p, Step: currentPoint.Step + p))
                            .ToList();
                        break;
                }

                currentPoint = move.Last();
                wire.UnionWith(move);
            }
            return wire;
        }
    }
}
