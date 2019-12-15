namespace Day15
{
    public static class DirectionExtensions
    {
        public static Direction TurnAround(this Direction currentDirection)
        {
            return currentDirection switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.West => Direction.East,
                Direction.East => Direction.West,
                _ => default
            };
        }
    }
}