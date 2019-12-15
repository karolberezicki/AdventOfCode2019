using System;
using System.Collections.Generic;
using System.Linq;

namespace Day14
{
    public class Program
    {
        private static Dictionary<string, Recipe> _recipes;
        private const long CargoHold = 1000000000000L;

        public static void Main()
        {
            var input = System.IO.File.ReadAllLines("input.txt")
                .ToList();

            _recipes = input
                .Select(line => new Recipe(line))
                .ToDictionary(r => r.OutputChemical, r => r);

            var part1 = FindRequiredOre(1);
            var part2 = CargoHold / part1;
            var usedOre = 0L;
            while (usedOre <= CargoHold)
            {
                usedOre = FindRequiredOre(part2 + 1);
                part2 = Math.Max(part2, part2 * CargoHold / usedOre);
            }

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        private static long FindRequiredOre(long fuelAmount)
        {
            var requiredChemicals = _recipes
                .Select(r => r.Value.OutputChemical)
                .ToDictionary(k => k, v => 0L);

            requiredChemicals["FUEL"] = fuelAmount;
            requiredChemicals["ORE"] = 0;

            while (requiredChemicals.Any(chemical => chemical.Key != "ORE" && chemical.Value > 0))
            {
                var requiredChemical = requiredChemicals.First(chemical => chemical.Key != "ORE" && chemical.Value > 0).Key;
                var requiredAmount = requiredChemicals[requiredChemical];
                var multiplier = Math.Max(requiredAmount / _recipes[requiredChemical].OutputCount, 1);

                requiredChemicals[requiredChemical] -= _recipes[requiredChemical].OutputCount * multiplier;
                foreach (var (ingredientName, recipeAmount) in _recipes[requiredChemical].Ingredients)
                {
                    requiredChemicals[ingredientName] += recipeAmount * multiplier;
                }
            }

            var requiredOre = requiredChemicals["ORE"];
            return requiredOre;
        }
    }
}