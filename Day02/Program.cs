using System;
using System.Collections.Generic;
using System.Linq;

namespace Day02
{
    public class Program
    {
        private const int ApolloLandingDate = 19690720;

        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");
            var memoryState = input.Split(",")
                .Select(int.Parse)
                .ToList();

            var noun = 12;
            var verb = 2;

            var part1 = RunIntCode(memoryState, noun, verb);

            while (RunIntCode(memoryState, noun + 1, verb) < ApolloLandingDate)
            {
                noun++;
            }

            while (RunIntCode(memoryState, noun, verb + 1) <= ApolloLandingDate)
            {
                verb++;
            }

            var part2 = 100 * noun + verb;

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        private static int RunIntCode(IEnumerable<int> memoryState, int noun, int verb)
        {
            var intCode = memoryState.ToList();

            var instructionPointer = 0;

            intCode[1] = noun;
            intCode[2] = verb;

            while (true)
            {
                var instruction = intCode[instructionPointer];

                switch (instruction)
                {
                    case 1:
                        {
                            var address1 = intCode[instructionPointer + 1];
                            var address2 = intCode[instructionPointer + 2];
                            var resultAddress = intCode[instructionPointer + 3];
                            var sum = intCode[address1] + intCode[address2];
                            intCode[resultAddress] = sum;
                            break;
                        }
                    case 2:
                        {
                            var address1 = intCode[instructionPointer + 1];
                            var address2 = intCode[instructionPointer + 2];
                            var resultAddress = intCode[instructionPointer + 3];
                            var product = intCode[address1] * intCode[address2];
                            intCode[resultAddress] = product;
                            break;
                        }
                    case 99:
                        return intCode[0];
                }

                instructionPointer += 4;
            }
        }
    }
}
