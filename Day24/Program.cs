using System;
using System.Collections.Generic;

namespace Day24
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");
            var grid = ParseInput(input);

            var biodiversityHistory = new HashSet<long>();
            long part1;
            while (true)
            {
                var biodiversity = CalculateBiodiversityRating(grid);
                if (biodiversityHistory.Contains(biodiversity))
                {
                    part1 = biodiversity;
                    break;
                }
                biodiversityHistory.Add(biodiversity);
                grid = Life(grid);
            }

            Console.WriteLine($"Part1 {part1}");
        }

        private static Dictionary<(int X, int Y), bool> Life(Dictionary<(int X, int Y), bool> grid)
        {
            var updatedGrid = new Dictionary<(int X, int Y), bool>();

            foreach (var ((x, y), value) in grid)
            {
                var up = grid.ContainsKey((x, y - 1)) && grid[(x, y - 1)];
                var down = grid.ContainsKey((x, y + 1)) && grid[(x, y + 1)];
                var left = grid.ContainsKey((x - 1, y)) && grid[(x - 1, y)];
                var right = grid.ContainsKey((x + 1, y)) && grid[(x + 1, y)];

                var countAdjacent = (up ? 1 : 0) + (down ? 1 : 0) + (left ? 1 : 0) + (right ? 1 : 0);

                if (value && countAdjacent != 1)
                {
                    updatedGrid[(x, y)] = false;
                }
                else if (!value && (countAdjacent == 1 || countAdjacent == 2))
                {
                    updatedGrid[(x, y)] = true;
                }
                else
                {
                    updatedGrid[(x, y)] = value;
                }
            }

            return updatedGrid;
        }

        private static long CalculateBiodiversityRating(Dictionary<(int X, int Y), bool> grid)
        {
            var biodiversityRating = 0;
            var tileValue = 1;
            foreach (var (_, tile) in grid)
            {
                if (tile)
                {
                    biodiversityRating += tileValue;
                }
                tileValue *= 2;
            }
            return biodiversityRating;
        }

        private static Dictionary<(int X, int Y), bool> ParseInput(string input)
        {
            var grid = new Dictionary<(int X, int Y), bool>();
            var x = 0;
            var y = 0;
            foreach (var c in input)
            {
                if (c == '\n')
                {
                    x = 0;
                    y++;
                    continue;
                }
                grid[(x, y)] = c == '#';
                x++;
            }
            return grid;
        }
    }
}
