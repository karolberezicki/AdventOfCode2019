using System;
using System.Linq;

namespace Day01
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllLines("input.txt")
                .Select(int.Parse)
                .ToList();

            var part1 = input.Sum(GetFuelRequirement);
            var part2 = input.Sum(GetFuelRequirementWithRequiredFuel);

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        public static int GetFuelRequirement(int moduleMass)
        {
            return moduleMass / 3 - 2;
        }

        public static int GetFuelRequirementWithRequiredFuel(int moduleMass)
        {
            var fuelRequirement = GetFuelRequirement(moduleMass);

            if (fuelRequirement > 0)
            {
                return fuelRequirement + GetFuelRequirementWithRequiredFuel(fuelRequirement);
            }

            return 0;
        }
    }
}
