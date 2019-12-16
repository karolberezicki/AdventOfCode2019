using System;
using System.Collections.Generic;
using System.Linq;

namespace Day16
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");
            var signal = input
                .Trim().Select(c => c - '0').ToList();

            signal = FlawedFrequencyTransmission(signal);

            var part1 = string.Join("", signal.Take(8));

            Console.WriteLine($"Part1 {part1}");
            //Console.WriteLine($"Part2 {part2}");
        }

        private static List<int> FlawedFrequencyTransmission(List<int> signal)
        {
            var basePattern = new List<int> { 0, 1, 0, -1 };

            for (var phase = 1; phase <= 100; phase++)
            {
                var updatedSignal = new List<int>();

                for (var patternIndex = 1; patternIndex <= signal.Count; patternIndex++)
                {
                    var pattern = new List<int>();
                    var basePatternIndex = 0;
                    while (pattern.Count < signal.Count + 1)
                    {
                        pattern.AddRange(Enumerable.Repeat(basePattern[basePatternIndex], patternIndex));
                        basePatternIndex++;
                        basePatternIndex = basePatternIndex >= basePattern.Count ? 0 : basePatternIndex;
                    }

                    pattern.RemoveAt(0);

                    var output = new List<int>();
                    for (var index = 0; index < signal.Count; index++)
                    {
                        output.Add(signal[index] * pattern[index]);
                    }

                    updatedSignal.Add(Math.Abs(output.Sum()) % 10);
                }

                signal = updatedSignal;
            }

            return signal;
        }
    }
}
