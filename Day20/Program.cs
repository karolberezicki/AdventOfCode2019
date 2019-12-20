using System;
using System.Collections.Generic;
using System.Linq;

namespace Day20
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllLines("input.txt")
                .ToList();

            var maze = GenerateMaze(input);

            var labels = new Dictionary<string, HashSet<(int X, int Y)>>();
            var portals = new Dictionary<(int X, int Y), (int X, int Y)>();


            foreach (var (x, y) in maze.Keys)
            {
                if (maze.ContainsKey((x, y + 1)) && char.IsLetter(maze[(x, y)]) && char.IsLetter(maze[(x, y + 1)]))
                {
                    var key = $"{maze[(x, y)]}{maze[(x, y + 1)]}";
                    if (!labels.ContainsKey(key))
                    {
                        labels[key] = new HashSet<(int X, int Y)>();
                    }

                    if (maze.ContainsKey((x, y + 2)) && maze[(x, y + 2)] == '.')
                    {
                        labels[key].Add((x, y + 2));

                    }
                    else if (maze.ContainsKey((x, y - 2)) && maze[(x, y - 2)] == '.')
                    {
                        labels[key].Add((x, y - 2));
                    }


                }

                if (maze.ContainsKey((x + 1, y)) && char.IsLetter(maze[(x, y)]) && char.IsLetter(maze[(x + 1, y)]))
                {
                    var key = $"{maze[(x, y)]}{maze[(x + 1, y)]}";
                    if (!labels.ContainsKey(key))
                    {
                        labels[key] = new HashSet<(int X, int Y)>();
                    }
                    if (maze.ContainsKey((x + 2, y)) && maze[(x + 2, y)] == '.')
                    {
                        labels[key].Add((x + 2, y));
                    }
                    else if (maze.ContainsKey((x - 2, y)) && maze[(x - 2, y)] == '.')
                    {
                        labels[key].Add((x - 2, y));
                    }
                }
            }

            var start = (0, 0);
            var target = (0, 0);

            foreach (var label in labels)
            {
                switch (label.Key)
                {
                    case "AA":
                        start = label.Value.First();
                        break;
                    case "ZZ":
                        target = label.Value.First();
                        break;
                    default:
                        portals.Add(label.Value.First(), label.Value.Last());
                        portals.Add(label.Value.Last(), label.Value.First());
                        break;
                }
            }

            var part1 = FindShortestDistance(maze, portals, start, target);

            Console.WriteLine($"Part1 {part1}");
        }

        private static int FindShortestDistance(Dictionary<(int X, int Y), char> map, Dictionary<(int X, int Y), (int X, int Y)> portals, (int X, int Y) start, (int X, int Y) target)
        {
            var statesQueue = new Queue<(int X, int Y, int Distance)>(new[] { (start.X, start.Y, 0) });
            (int X, int Y, int Distance) finishState = (0, 0, int.MaxValue);
            var visited = new HashSet<(int X, int Y)> { start };

            while (statesQueue.Count > 0)
            {
                var currentState = statesQueue.Dequeue();

                if (currentState.X == target.X && currentState.Y == target.Y && currentState.Distance < finishState.Distance)
                {
                    finishState = currentState;
                }
                else
                {
                    var possibleMoves = new HashSet<(int X, int Y, int Distance)>
                    {
                        (currentState.X + 1, currentState.Y, currentState.Distance + 1),
                        (currentState.X - 1, currentState.Y, currentState.Distance + 1),
                        (currentState.X, currentState.Y + 1, currentState.Distance + 1),
                        (currentState.X, currentState.Y - 1, currentState.Distance + 1)
                    };

                    if (portals.ContainsKey((currentState.X, currentState.Y)))
                    {
                        var (portalDestinationX, portalDestinationY) = portals[(currentState.X, currentState.Y)];
                        possibleMoves.Add((portalDestinationX, portalDestinationY, currentState.Distance + 2));
                    }

                    if (finishState != default)
                    {
                        possibleMoves =
                            new HashSet<(int X, int Y, int Distance)>(possibleMoves.Where(move =>
                                move.Distance < finishState.Distance));
                    }

                    foreach (var newState in possibleMoves)
                    {
                        var position = (newState.X, newState.Y);
                        if (map.ContainsKey(position)
                            && map[position] == '.'
                            && !visited.Contains(position)
                            && !statesQueue.Contains(newState))
                        {
                            visited.Add(position);
                            statesQueue.Enqueue(newState);
                        }
                    }
                }
            }

            return finishState.Distance;
        }

        private static Dictionary<(int X, int Y), char> GenerateMaze(List<string> input)
        {
            var maze = new Dictionary<(int X, int Y), char>();
            for (var y = 0; y < input.Count; y++)
            {
                for (var x = 0; x < input[0].Length; x++)
                {
                    maze[(x, y)] = input[y][x];
                }
            }

            return maze;
        }
    }
}
