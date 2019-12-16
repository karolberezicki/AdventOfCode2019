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

            var signal2 = input
                .Trim().Select(c => c - '0').ToList();

            signal2 = Enumerable.Repeat(signal2, 10000).SelectMany(n => n).ToList();
            var messageOffset = int.Parse(string.Join("", input.Take(7)));

            signal2 = FlawedFrequencyTransmission2(signal2, messageOffset);

            var part2 = string.Join("", signal2.Take(8));
            Console.WriteLine($"Part2 {part2}");
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

        private static List<int> FlawedFrequencyTransmission2(List<int> signal, int messageOffset)
        {
            var index = 0;
            var signalDict = signal.Skip(messageOffset)
                .ToDictionary(item => index++);

            for (var phase = 1; phase <= 100; phase++)
            {
                for (var i = signalDict.Count - 1; i >= 0; i--)
                {
                    var previousDigit = signalDict.ContainsKey(i + 1) ? signalDict[i + 1] : 0;
                    signalDict[i] = Math.Abs(previousDigit + signalDict[i]) % 10;
                }
            }

            return signalDict.Values.ToList();
        }
    }
}
