using System;
using System.Collections.Generic;
using System.Linq;

namespace Day24
{
    public class Program
    {
        private const int Levels = 120;
        private const int ObservationTime = 200;

        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");

            var part1 = Part1(input);
            Console.WriteLine($"Part1 {part1}");

            var part2 = Part2(input);
            Console.WriteLine($"Part2 {part2}");
        }

        private static long Part1(string input)
        {
            var grid = ParseInput(input);
            var biodiversityHistory = new HashSet<long>();
            while (true)
            {
                var biodiversity = CalculateBiodiversityRating(grid);
                if (biodiversityHistory.Contains(biodiversity))
                {
                    return biodiversity;
                }

                biodiversityHistory.Add(biodiversity);
                grid = Life(grid);
            }
        }

        private static int Part2(string input)
        {
            var grid = InitGrid();
            grid[0] = ParseInput(input);

            for (var minutes = 0; minutes < ObservationTime; minutes++)
            {
                grid = MultiLevelLife(grid);
            }

            return grid.Select(l => l.Value.Count(n => n.Value)).Sum();
        }

        private static Dictionary<(int X, int Y), bool> Life(IReadOnlyDictionary<(int X, int Y), bool> grid)
        {
            var updatedGrid = new Dictionary<(int X, int Y), bool>();

            foreach (var ((x, y), value) in grid)
            {
                var up = grid.ContainsKey((x, y - 1)) && grid[(x, y - 1)];
                var down = grid.ContainsKey((x, y + 1)) && grid[(x, y + 1)];
                var left = grid.ContainsKey((x - 1, y)) && grid[(x - 1, y)];
                var right = grid.ContainsKey((x + 1, y)) && grid[(x + 1, y)];
                updatedGrid[(x, y)] = GetTile(value, up, down, left, right);
            }

            return updatedGrid;
        }

        private static Dictionary<int, Dictionary<(int X, int Y), bool>> MultiLevelLife(IReadOnlyDictionary<int, Dictionary<(int X, int Y), bool>> grid)
        {
            var gridTimePlusOne = InitGrid();
            for (var level = -Levels + 1; level < Levels; level++)
            {
                for (var x = 0; x < 5; x++)
                {
                    for (var y = 0; y < 5; y++)
                    {
                        if ((x, y) == (2, 2))
                        {
                            continue; // tile 13, ignore
                        }

                        gridTimePlusOne[level][(x, y)] = GetTile(grid[level][(x, y)], GetAdjacent(grid, level, (x, y)));
                    }
                }
            }

            return gridTimePlusOne;
        }

        private static bool[] GetAdjacent(IReadOnlyDictionary<int, Dictionary<(int X, int Y), bool>> grid, int level, (int X, int Y) position)
        {
            var adjacent = new List<bool>
            {
                grid[level].ContainsKey((position.X, position.Y-1)) && grid[level][(position.X, position.Y-1)], // up
                grid[level].ContainsKey((position.X, position.Y+1)) && grid[level][(position.X, position.Y+1)], // down
                grid[level].ContainsKey((position.X-1, position.Y)) && grid[level][(position.X-1, position.Y)], // left
                grid[level].ContainsKey((position.X+1, position.Y)) && grid[level][(position.X+1, position.Y)], // right
                position.Y == 0 && grid[level + 1][(2, 1)],
                position.X == 0 && grid[level + 1][(1, 2)],
                position.X == 4 && grid[level + 1][(3, 2)],
                position.Y == 4 && grid[level + 1][(2, 3)]
            };

            switch (position)
            {
                case (2, 1):
                    adjacent.AddRange(Enumerable.Range(0, 5).Select(i => grid[level - 1][(i, 0)]));
                    break;
                case (1, 2):
                    adjacent.AddRange(Enumerable.Range(0, 5).Select(i => grid[level - 1][(0, i)]));
                    break;
                case (3, 2):
                    adjacent.AddRange(Enumerable.Range(0, 5).Select(i => grid[level - 1][(4, i)]));
                    break;
                case (2, 3):
                    adjacent.AddRange(Enumerable.Range(0, 5).Select(i => grid[level - 1][(i, 4)]));
                    break;
            }

            return adjacent.ToArray();
        }

        private static bool GetTile(bool value, params bool[] adjacent)
        {
            var countAdjacent = adjacent.Select(a => a ? 1 : 0).Sum();

            if (value && countAdjacent != 1)
            {
                return false;
            }
            if (!value && (countAdjacent == 1 || countAdjacent == 2))
            {
                return true;
            }
            return value;
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

        private static Dictionary<int, Dictionary<(int X, int Y), bool>> InitGrid()
        {
            var grid = new Dictionary<int, Dictionary<(int X, int Y), bool>>();
            for (var level = -Levels; level <= Levels; level++)
            {
                grid[level] = new Dictionary<(int X, int Y), bool>();
                for (var x = 0; x < 5; x++)
                {
                    for (var y = 0; y < 5; y++)
                    {
                        grid[level][(x, y)] = false;
                    }
                }
            }
            return grid;
        }
    }
}
