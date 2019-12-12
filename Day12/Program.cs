using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day12
{
    public class Program
    {
        public static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            var part1 = Part1(input);

            Console.WriteLine($"Part1 {part1}");

            var cycleX = GetCycle(input, Dimension.X);
            var cycleY = GetCycle(input, Dimension.Y);
            var cycleZ = GetCycle(input, Dimension.Z);

            var part2 = LeastCommonMultiple(LeastCommonMultiple(cycleX, cycleY), cycleZ);

            Console.WriteLine($"Part2 {part2}");
        }

        private static long GetCycle(IEnumerable<string> input, Dimension dimension)
        {
            var moons = GetMoons(input);

            long step = 0;

            while (true)
            {
                for (var i = 0; i < moons.Count; i++)
                {
                    for (var j = i + 1; j < moons.Count; j++)
                    {
                        moons[i].ApplyGravity(dimension, moons[j]);
                    }
                }

                foreach (var moon in moons)
                {
                    moon.ApplyVelocity(dimension);
                }

                step++;

                if (moons.Select(m => m.Velocity[dimension]).All(v => v == 0))
                {
                    return step * 2;
                }
            }
        }

        private static int Part1(IEnumerable<string> input)
        {
            var moons = GetMoons(input);

            const int stepsCount = 1000;

            for (var step = 0; step < stepsCount; step++)
            {
                for (var i = 0; i < moons.Count; i++)
                {
                    for (var j = i + 1; j < moons.Count; j++)
                    {
                        moons[i].ApplyGravity(moons[j]);
                    }
                }

                foreach (var moon in moons)
                {
                    moon.ApplyVelocity();
                }
            }

            var part1 = moons.Select(m => m.TotalEnergy).Sum();
            return part1;
        }

        private static List<Moon> GetMoons(IEnumerable<string> input)
        {
            var regex = new Regex(@"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>");
            var moons = input
                .Select(l => regex.Match(l).Groups.Values.Select(v => v.Value).ToArray())
                .Select(l => new Moon(int.Parse(l[1]), int.Parse(l[2]), int.Parse(l[3])))
                .ToList();
            return moons;
        }

        private static long GreatestCommonDivisor(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            // Pull out remainders.
            while (true)
            {
                long remainder = a % b;
                if (remainder == 0) return b;
                a = b;
                b = remainder;
            }
        }

        private static long LeastCommonMultiple(long a, long b)
        {
            return a * b / GreatestCommonDivisor(a, b);
        }
    }
}
