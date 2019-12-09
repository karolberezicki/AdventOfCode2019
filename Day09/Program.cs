using System;
using System.Linq;

namespace Day09
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");

            var memoryState = input.Split(",")
                .Select(long.Parse)
                .ToList();

            var icc = new IntCodeComputer(memoryState, 1);
            icc.RunTillHalt();

            var part1 = icc.Output.First();

            icc = new IntCodeComputer(memoryState, 2);
            icc.RunTillHalt();

            var part2 = icc.Output.First();

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }
    }
}
