using IntCode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day15
{
    public class Program
    {

        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");

            var memoryState = input.Split(",")
                .Select(long.Parse)
                .ToList();

            var map = GenerateMap(memoryState);
            DisplayMaze(map);
        }

        private static Dictionary<(int X, int Y), StatusCode> GenerateMap(IEnumerable<long> memoryState)
        {
            var map = new Dictionary<(int X, int Y), StatusCode>();
            var icc = new IntCodeComputer(memoryState);
            var currentPosition = (X: 0, Y: 0);
            var visited = new Dictionary<(int X, int Y), int> { [currentPosition] = 1 };

            while (visited.Values.Min() < 3)
            {
                var possibleMoves =
                    ((Direction[])Enum.GetValues(typeof(Direction)))
                    .Select(d => (Direction: d, StatusCode: CheckDirection(icc, d)))
                    .ToList();

                var nextMove = (Position: currentPosition, VisitCount: int.MaxValue, Direction: Direction.North);
                foreach (var (direction, statusCode) in possibleMoves)
                {
                    var movePosition = direction switch
                    {
                        Direction.North => (currentPosition.X, currentPosition.Y - 1),
                        Direction.South => (currentPosition.X, currentPosition.Y + 1),
                        Direction.West => (currentPosition.X - 1, currentPosition.Y),
                        Direction.East => (currentPosition.X + 1, currentPosition.Y),
                        _ => default
                    };

                    map[movePosition] = statusCode;
                    var visitCount = visited.ContainsKey(movePosition) ? visited[movePosition] : 0;
                    if (statusCode != StatusCode.Wall && nextMove.VisitCount >= visitCount)
                    {
                        nextMove = (movePosition, visitCount, direction);
                    }
                }

                Move(icc, nextMove.Direction);
                currentPosition = nextMove.Position;
                visited[currentPosition] = nextMove.VisitCount + 1;
            }

            return map;
        }

        private static StatusCode CheckDirection(IntCodeComputer icc, Direction direction)
        {
            var status = Move(icc, direction);
            if (status != StatusCode.Wall)
            {
                Move(icc, direction.TurnAround());
            }
            return status;
        }

        private static StatusCode Move(IntCodeComputer icc, Direction direction)
        {
            icc.RunIntCode(BreakMode.Input);
            icc.Inputs.Enqueue((long)direction);
            icc.RunIntCode();
            return (StatusCode)icc.Output.Dequeue();
        }

        private static void DisplayMaze(Dictionary<(int X, int Y), StatusCode> maze)
        {
            Console.CursorVisible = false;
            Console.Clear();

            var xOffset = Math.Min(0, maze.Keys.Select(m => m.X).Min()) * -1;
            var yOffset = Math.Min(0, maze.Keys.Select(m => m.Y).Min()) * -1;

            var cursorEndPosition = Math.Max(1, maze.Keys.Select(m => m.Y).Max()) + yOffset + 1;

            foreach (var ((x, y), statusCode) in maze)
            {
                Console.SetCursorPosition(x + xOffset, y + yOffset);
                switch (statusCode)
                {
                    case StatusCode.Wall:
                        Console.Write("#");
                        break;
                    case StatusCode.Move:
                        Console.Write(".");
                        break;
                    case StatusCode.OxygenSystem:
                        Console.Write("2");
                        break;
                    default:
                        Console.Write("?");
                        break;
                }
            }

            Console.SetCursorPosition(0, cursorEndPosition);
        }
    }
}
