using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day12
{
    public class Program
    {
        public static void Main()
        {
            var regex = new Regex(@"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>");
            var moons = File.ReadAllLines("input.txt")
                .Select(l => regex.Match(l).Groups.Values.Select(v => v.Value).ToArray())
                .Select(l => new Moon(int.Parse(l[1]), int.Parse(l[2]), int.Parse(l[3])))
                .ToList();

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

            Console.WriteLine($"Part1 {part1}");
        }
    }
}
