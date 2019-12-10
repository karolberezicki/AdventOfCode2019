using System;
using System.Collections.Generic;
using System.Linq;

namespace Day10
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllLines("input.txt")
                .ToList();

            var asteroidsMap = input.Select(l => l.ToCharArray()).ToArray();
            var asteroids = new HashSet<(int X, int Y)>(
                asteroidsMap.Select((row, y) =>
                        row.Select((value, x) => (x, y, value)))
                    .SelectMany(r => r)
                    .Where(point => point.value == '#')
                    .Select(point => (point.x, point.y)));

            var part1 = 0;
            var instantMonitoringStationLocation = (-1, -1);
            foreach (var currentAsteroid in asteroids)
            {
                var rays = GetRays(asteroids, currentAsteroid);

                var distinctPointsAtRay = rays.Select(r => r.RayAngle).Distinct().Count();

                if (distinctPointsAtRay > part1)
                {
                    part1 = distinctPointsAtRay;
                    instantMonitoringStationLocation = currentAsteroid;
                }
            }

            var imsRays = GetRays(asteroids, instantMonitoringStationLocation);
            var asteroidsByDirection = imsRays.GroupBy(r => r.RayAngle)
                .Select(g => (
                    RayAngle: g.Key,
                    Targets: new Queue<(int X, int Y)>(
                        g.Select(asteroid => (asteroid.X, asteroid.Y))
                            .OrderBy(asteroid => Distance(instantMonitoringStationLocation, asteroid))
                        )
                    )
                ).OrderBy(g => g.RayAngle > Math.PI / 2)
                .ThenByDescending(g => g.RayAngle)
                .ToList();

            var vaporized = 0;
            var rayCounter = 0;
            var lastTarget = (X: -1, Y: -1);
            while (vaporized < 200)
            {

                lastTarget = asteroidsByDirection[rayCounter].Targets.Dequeue();
                rayCounter++;
                rayCounter = rayCounter >= asteroidsByDirection.Count ? 0 : rayCounter;
                vaporized++;
            }

            var part2 = lastTarget.X * 100 + lastTarget.Y;

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        private static List<(int X, int Y, double RayAngle)> GetRays(HashSet<(int X, int Y)> asteroids, (int X, int Y) targetAsteroid)
        {
            // https://math.stackexchange.com/questions/707673/find-angle-in-degrees-from-one-point-to-another-in-2d-space

            var rays = asteroids.Except(new[] { targetAsteroid })
                .Select(asteroid =>
                    (asteroid.X, asteroid.Y,
                        RayAngle: -Math.Atan2(asteroid.Y - targetAsteroid.Y, asteroid.X - targetAsteroid.X)))
                .ToList();
            return rays;
        }

        private static int Distance((int X, int Y) pointA, (int X, int Y) pointB)
        {
            return Math.Abs(pointA.X - pointB.X) + Math.Abs(pointA.Y - pointB.Y);
        }
    }
}
