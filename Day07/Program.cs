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

            var amplifiersWithSignals = amplifiersPermutations.Select(permutation =>
            {
                var inputValue = 0;
                foreach (var phaseSetting in permutation)
                {
                    var amp = new IntCodeComputer(memoryState);
                    amp.Inputs.Add(phaseSetting);
                    amp.Inputs.Add(inputValue);
                    amp.RunIntCode();
                    inputValue = amp.Output.Last();
                }

                return (Amplifier: permutation, Signal: inputValue);
            });

            return amplifiersWithSignals.OrderByDescending(t => t.Signal).First().Signal;
        }

        private static int Part2(IReadOnlyCollection<int> memoryState)
        {
            var amplifiers = new List<int> { 9, 8, 7, 6, 5 };
            var amplifiersPermutations = Permutations.GeneratePermutations(amplifiers);

            var amplifiersWithSignals = new List<(List<int> Amplifier, int Signal)>();

            foreach (var permutation in amplifiersPermutations)
            {
                var amps = new List<IntCodeComputer>();
                foreach (var phaseSetting in permutation)
                {
                    var amp = new IntCodeComputer(memoryState);
                    amp.Inputs.Add(phaseSetting);
                    amps.Add(amp);
                }

                var inputValue = 0;

                while (!amps.Last().IsHalted)
                {
                    foreach (var amp in amps)
                    {
                        amp.Inputs.Add(inputValue);
                        amp.RunIntCode();
                        inputValue = amp.Output.Last();
                    }
                }

                amplifiersWithSignals.Add((Amplifier: permutation, Signal: inputValue));
            }

            return amplifiersWithSignals.OrderByDescending(t => t.Signal).First().Signal;
        }
    }
}
