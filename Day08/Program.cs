using System;
using System.Collections.Generic;
using System.Linq;

namespace Day08
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt").Trim().ToList();
            const int width = 25;
            const int height = 6;

            var image = ParseImage(input, width, height);

            var layersWithZerosCount = image
                .Select(l => (LayerData: l, ZeroCount: l.Sum(p => p.Count(c => c == '0'))))
                .OrderBy(l => l.ZeroCount)
                .ToList();

            var layerWithFewestZeros = layersWithZerosCount.First().LayerData;
            var part1 = layerWithFewestZeros.Sum(p => p.Count(c => c == '1')) * layerWithFewestZeros.Sum(p => p.Count(c => c == '2'));

            Console.WriteLine($"Part1 {part1}");

            Console.WriteLine("Part2");

            foreach (var h in Enumerable.Range(0, height))
            {
                var row = Enumerable.Range(0, width)
                    .Select(w => Enumerable.Range(0, 100)
                        .Select(i => image[i][h][w])
                        .First(p => p != '2'))
                    .Select(pixel => pixel == '1' ? '█' : ' ');
                Console.WriteLine(string.Concat(row));
            }
        }

        private static List<List<List<char>>> ParseImage(IReadOnlyCollection<char> input, int width, int height)
        {
            var layers = input.Count / (width * height);
            var image = new List<List<List<char>>>();
            for (var l = 0; l < layers; l++)
            {
                var layerData = input
                    .Skip(l * width * height)
                    .Take(width * height)
                    .ToList();

                var layer = new List<List<char>>();
                for (var h = 0; h < height; h++)
                {
                    var skipRow = width * h;
                    var row = layerData.Skip(skipRow).Take(width).ToList();
                    layer.Add(row);
                }

                image.Add(layer);
            }
            return image;
        }
    }
}
