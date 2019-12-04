using System;
using System.Linq;

namespace Day04
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int lowerRange = 183564;
            const int upperRange = 657474;

            var criteriaMatches = Enumerable.Range(lowerRange, upperRange - lowerRange)
                .Select(MatchCriteria)
                .ToList();

            var part1 = criteriaMatches.Count(r => r.rule1 && r.rule2);
            var part2 = criteriaMatches.Count(r => r.rule1 && r.rule3);

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        private static (bool rule1, bool rule2, bool rule3) MatchCriteria(int password)
        {
            var passwordDigits = password.ToString().ToCharArray();
            var countOfDigits = passwordDigits.GroupBy(c => c).Select(g => g.Count()).ToList();

            var rule1 = passwordDigits.OrderBy(d => d).SequenceEqual(passwordDigits);
            var rule2 = countOfDigits.OrderByDescending(c => c).First() >= 2;
            var rule3 = countOfDigits.Contains(2);

            var matches = (rule1, rule2, rule3);
            return matches;
        }
    }
}
