using IntCode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day15
{
    public class Program
    {
        private static IntCodeComputer _icc;
        private static (int X, int Y) _currentPosition;
        private static Dictionary<(int X, int Y), int> _visited;
        private static Dictionary<(int X, int Y), StatusCode> _map;

        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");

            var memoryState = input.Split(",")
                .Select(long.Parse)
                .ToList();

            _icc = new IntCodeComputer(memoryState);
            _currentPosition = (X: 0, Y: 0);
            _visited = new Dictionary<(int X, int Y), int> { [_currentPosition] = 1 };
            _map = new Dictionary<(int X, int Y), StatusCode>();

            while (_visited.Values.Min() < 3)
            {
                var possibleMoves =
                    ((Direction[])Enum.GetValues(typeof(Direction)))
                    .Select(d => (Direction: d, StatusCode: CheckDirection(d)))
                    .ToList();

                var nextMove = (Position: (_currentPosition), VisitCount: int.MaxValue, Direction: Direction.North);
                foreach (var (direction, statusCode) in possibleMoves)
                {
                    var movePosition = direction switch
                    {
                        Direction.North => (_currentPosition.X, _currentPosition.Y - 1),
                        Direction.South => (_currentPosition.X, _currentPosition.Y + 1),
                        Direction.West => (_currentPosition.X - 1, _currentPosition.Y),
                        Direction.East => (_currentPosition.X + 1, _currentPosition.Y),
                        _ => default
                    };

                    _map[movePosition] = statusCode;
                    var visitCount = _visited.ContainsKey(movePosition) ? _visited[movePosition] : 0;
                    if (statusCode != StatusCode.Wall && nextMove.VisitCount >= visitCount)
                    {
                        nextMove = (movePosition, visitCount, direction);
                    }
                }

                Move(nextMove.Direction);
                _currentPosition = nextMove.Position;
                _visited[_currentPosition] = nextMove.VisitCount + 1;
            }

            DisplayMaze(_map);
        }

        public static StatusCode CheckDirection(Direction direction)
        {
            var status = Move(direction);
            if (status != StatusCode.Wall)
            {
                Move(direction.TurnAround());
            }
            return status;
        }

        public static StatusCode Move(Direction direction)
        {
            _icc.RunIntCode(BreakMode.Input);
            _icc.Inputs.Enqueue((long)direction);
            _icc.RunIntCode();
            return (StatusCode)_icc.Output.Dequeue();
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
