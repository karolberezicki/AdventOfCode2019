using System;
using System.Collections.Generic;
using System.Linq;

namespace Day07
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");
            var memoryState = input.Split(",")
                .Select(int.Parse)
                .ToList();

            var part1 = Part1(memoryState);
            var part2 = Part2(memoryState);

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        private static int Part1(IReadOnlyCollection<int> memoryState)
        {
            var amplifiers = new List<int> { 0, 1, 2, 3, 4 };
            var amplifiersPermutations = Permutations.GeneratePermutations(amplifiers);

            var signals = amplifiersPermutations.Select(permutation =>
            {
                var signal = 0;
                foreach (var phaseSetting in permutation)
                {
                    var amp = new IntCodeComputer(memoryState, phaseSetting, signal);
                    amp.RunIntCode();
                    signal = amp.Output.Last();
                }

                return signal;
            });

            return signals.Max();
        }

        private static int Part2(IReadOnlyCollection<int> memoryState)
        {
            var amplifiers = new List<int> { 9, 8, 7, 6, 5 };
            var amplifiersPermutations = Permutations.GeneratePermutations(amplifiers);

            var signals = amplifiersPermutations.Select(permutation =>
            {
                var amps = permutation
                    .Select(phaseSetting => new IntCodeComputer(memoryState, phaseSetting))
                    .ToList();

                var signal = 0;

                while (!amps.Last().IsHalted)
                {
                    foreach (var amp in amps)
                    {
                        amp.Inputs.Add(signal);
                        amp.RunIntCode();
                        signal = amp.Output.Last();
                    }
                }

                return signal;
            });

            return signals.Max();
        }
    }
}
