using System;
using System.Collections.Generic;
using System.Linq;
using IntCode;

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

            var part1 = new HashSet<(int X, int Y)>(paintedPanels.Select(p => (p.X, p.Y))).Count;

            Console.WriteLine($"Part1 {part1}");
        }

        private static List<(int X, int Y, int Color)> PaintHull(List<long> memoryState, bool startOnWhite = false)
        {
            var paintedPanels = new List<(int X, int Y, int Color)>();
            var currentDirection = Direction.Up;
            var currentPosition = (X: 100, Y: 100);
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
                currentDirection = Turn(currentDirection, turnDirection);
                currentPosition = Move(currentPosition, currentDirection);
            }

            return paintedPanels;
        }

        public static (int X, int Y) Move((int X, int Y) currentPosition, Direction direction)
        {
            var (x, y) = currentPosition;
            switch (direction)
            {
                case Direction.Up:
                    return (x, y + 1);
                case Direction.Right:
                    return (x + 1, y);
                case Direction.Down:
                    return (x, y - 1);
                case Direction.Left:
                    return (x - 1, y);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public static Direction Turn(Direction currentDirection, int turnDirection)
        {
            if (turnDirection == 0) // Left 90 degrees
            {
                switch (currentDirection)
                {
                    case Direction.Up:
                        return Direction.Left;
                    case Direction.Right:
                        return Direction.Up;
                    case Direction.Down:
                        return Direction.Right;
                    case Direction.Left:
                        return Direction.Down;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null);
                }
            }
            else // Right 90 degrees
            {
                switch (currentDirection)
                {
                    case Direction.Up:
                        return Direction.Right;
                    case Direction.Right:
                        return Direction.Down;
                    case Direction.Down:
                        return Direction.Left;
                    case Direction.Left:
                        return Direction.Up;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null);
                }
            }
        }
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}
