namespace Day11
{
    public static class DirectionExtensions
    {
        public static Direction Turn(this Direction currentDirection, int turnDirection)
        {
            if (turnDirection == 0) // Left 90 degrees
            {
                return currentDirection switch
                {
                    Direction.Up => Direction.Left,
                    Direction.Right => Direction.Up,
                    Direction.Down => Direction.Right,
                    Direction.Left => Direction.Down,
                    _ => currentDirection
                };
            }

            // Right 90 degrees
            return currentDirection switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => currentDirection
            };
        }

        public static (int X, int Y) Move(this Direction direction, (int X, int Y) currentPosition)
        {
            var (x, y) = currentPosition;
            return direction switch
            {
                Direction.Up => (x, y + 1),
                Direction.Right => (x + 1, y),
                Direction.Down => (x, y - 1),
                Direction.Left => (x - 1, y),
                _ => (x, y)
            };
        }
    }
}