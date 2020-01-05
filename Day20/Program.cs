using System;
using System.Collections.Generic;
using System.Linq;

namespace Day20
{
    public class Program
    {
        private static Dictionary<(int X, int Y), char> _maze;
        private static Dictionary<string, HashSet<(int X, int Y)>> _labels;
        private static Dictionary<(int X, int Y), (int X, int Y)> _portals;
        private static (int XMin, int YMin, int XMax, int YMax) _boundaries;

        public static void Main()
        {
            var input = System.IO.File.ReadAllLines("input.txt")
                .ToList();

            GenerateMaze(input);
            GenerateLabels();
            GeneratePortals();

            var start = _labels["AA"].First();
            var target = _labels["ZZ"].First();

            var part1 = Part1(start, target);
            Console.WriteLine($"Part1 {part1}");

            var part2 = Part2(start, target);
            Console.WriteLine($"Part2 {part2}");
        }

        private static int Part1((int X, int Y) start, (int X, int Y) target)
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

                    if (_portals.ContainsKey((currentState.X, currentState.Y)))
                    {
                        var (portalDestinationX, portalDestinationY) = _portals[(currentState.X, currentState.Y)];
                        possibleMoves.Add((portalDestinationX, portalDestinationY, currentState.Distance + 1));
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
                        var toExplore = _maze.ContainsKey(position)
                                          && _maze[position] == '.'
                                          && !visited.Contains(position)
                                          && !statesQueue.Contains(newState);
                        if (toExplore)
                        {
                            visited.Add(position);
                            statesQueue.Enqueue(newState);
                        }
                    }
                }
            }

            return finishState.Distance;
        }

        private static int Part2((int X, int Y) start, (int X, int Y) target)
        {
            var statesQueue = new Queue<(int X, int Y, int Distance, int Level)>(new[] { (start.X, start.Y, 0, 0) });
            (int X, int Y, int Distance, int Level) finishState = (0, 0, int.MaxValue, 0);
            var visited = new HashSet<(int X, int Y, int Level)> { (start.X, start.Y, 0) };

            while (statesQueue.Count > 0)
            {
                var currentState = statesQueue.Dequeue();

                if (currentState.X == target.X && currentState.Y == target.Y && currentState.Distance < finishState.Distance && currentState.Level == 0)
                {
                    finishState = currentState;
                }
                else
                {
                    var possibleMoves = new HashSet<(int X, int Y, int Distance, int Level)>
                    {
                        (currentState.X + 1, currentState.Y, currentState.Distance + 1, currentState.Level),
                        (currentState.X - 1, currentState.Y, currentState.Distance + 1, currentState.Level),
                        (currentState.X, currentState.Y + 1, currentState.Distance + 1, currentState.Level),
                        (currentState.X, currentState.Y - 1, currentState.Distance + 1, currentState.Level)
                    };

                    if (_portals.ContainsKey((currentState.X, currentState.Y)))
                    {
                        var (portalDestinationX, portalDestinationY) = _portals[(currentState.X, currentState.Y)];
                        var portalLevel = IsOuterEdge((currentState.X, currentState.Y))
                            ? currentState.Level - 1
                            : currentState.Level + 1;

                        if (portalLevel >= 0)
                        {
                            possibleMoves.Add((portalDestinationX, portalDestinationY, currentState.Distance + 1, portalLevel));
                        }
                    }

                    if (finishState != default)
                    {
                        possibleMoves =
                            new HashSet<(int X, int Y, int Distance, int Level)>(possibleMoves.Where(move =>
                                move.Distance < finishState.Distance));
                    }

                    foreach (var newState in possibleMoves)
                    {
                        var positionCoordinates = (newState.X, newState.Y);
                        var position = (newState.X, newState.Y, newState.Level);
                        var toExplore = _maze.ContainsKey(positionCoordinates)
                                          && _maze[positionCoordinates] == '.'
                                          && !visited.Contains(position)
                                          && !statesQueue.Contains(newState);
                        if (toExplore)
                        {
                            visited.Add(position);
                            statesQueue.Enqueue(newState);
                        }
                    }
                }
            }

            return finishState.Distance;
        }

        private static bool IsOuterEdge((int X, int Y) coordinate)
        {
            return _boundaries.XMin == coordinate.X
                   || _boundaries.XMax == coordinate.X
                   || _boundaries.YMin == coordinate.Y
                   || _boundaries.YMax == coordinate.Y;
        }

        private static void GeneratePortals()
        {
            _portals = new Dictionary<(int X, int Y), (int X, int Y)>();
            foreach (var (label, points) in _labels)
            {
                switch (label)
                {
                    case "AA":
                    case "ZZ":
                        break;
                    default:
                        _portals.Add(points.First(), points.Last());
                        _portals.Add(points.Last(), points.First());
                        break;
                }
            }
        }

        private static void GenerateLabels()
        {
            _labels = new Dictionary<string, HashSet<(int X, int Y)>>();

            foreach (var (x, y) in _maze.Keys)
            {
                AssignLabel(x, y, 1, 0);
                AssignLabel(x, y, -1, 0);
                AssignLabel(x, y, 0, 1);
                AssignLabel(x, y, 0, -1);
            }
        }

        private static void AssignLabel(int x, int y, int dx, int dy)
        {
            if (
                _maze[(x, y)] == '.'
                && _maze.ContainsKey((x + dx, y + dy))
                && _maze.ContainsKey((x + dx * 2, y + dy * 2))
                && char.IsLetter(_maze[(x + dx, y + dy)])
                && char.IsLetter(_maze[(x + dx * 2, y + dy * 2)]))
            {
                var key = dx < 0 || dy < 0
                    ? $"{_maze[(x + dx * 2, y + dy * 2)]}{_maze[(x + dx, y + dy)]}"
                    : $"{_maze[(x + dx, y + dy)]}{_maze[(x + dx * 2, y + dy * 2)]}";

                if (!_labels.ContainsKey(key))
                {
                    _labels[key] = new HashSet<(int X, int Y)>();
                }

                _labels[key].Add((x, y));
            }
        }



        private static void GenerateMaze(List<string> input)
        {
            _maze = new Dictionary<(int X, int Y), char>();
            for (var y = 0; y < input.Count; y++)
            {
                for (var x = 0; x < input[0].Length; x++)
                {
                    _maze[(x, y)] = input[y][x];
                }
            }

            _boundaries = (
                XMin: _maze.Where(v => v.Value == '#').Select(c => c.Key.X).Min(),
                YMin: _maze.Where(v => v.Value == '#').Select(c => c.Key.Y).Min(),
                XMax: _maze.Where(v => v.Value == '#').Select(c => c.Key.X).Max(),
                YMax: _maze.Where(v => v.Value == '#').Select(c => c.Key.Y).Max());
        }
    }
}
