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

            var amplifiers = new List<int> { 0, 1, 2, 3, 4 };
            var amplifiersPermutations = Permutations.GeneratePermutations(amplifiers);

            var amplifiersWithSignals = amplifiersPermutations.Select(permutation =>
            {
                var run4 = RunIntCode(memoryState, permutation[0], 0).First();
                var run3 = RunIntCode(memoryState, permutation[1], run4).First();
                var run2 = RunIntCode(memoryState, permutation[2], run3).First();
                var run1 = RunIntCode(memoryState, permutation[3], run2).First();
                var run0 = RunIntCode(memoryState, permutation[4], run1).First();

                return (Amplifier: permutation, Signal: run0);
            });

            var part1 = amplifiersWithSignals.OrderByDescending(t => t.Signal).First().Signal;

            Console.WriteLine($"Part1 {part1}");
            //Console.WriteLine($"Part2 {part2}");
        }

        private static List<int> RunIntCode(IEnumerable<int> memoryState, params int[] inputs)
        {
            var intCode = memoryState.ToList();
            var output = new List<int>();

            var instructionPointer = 0;
            var inputPointer = 0;

            while (true)
            {
                var instruction = intCode[instructionPointer].ToString().PadLeft(5, '0');
                var opCode = GetNumber(instruction[3]) * 10 + GetNumber(instruction[4]);
                var modeParam1 = GetNumber(instruction[2]);
                var modeParam2 = GetNumber(instruction[1]);

                switch (opCode)
                {
                    case 1: // sum
                        {
                            var resultAddress = intCode[instructionPointer + 3];
                            var param1 = modeParam1 == 0 ? intCode[intCode[instructionPointer + 1]] : intCode[instructionPointer + 1];
                            var param2 = modeParam2 == 0 ? intCode[intCode[instructionPointer + 2]] : intCode[instructionPointer + 2];
                            intCode[resultAddress] = param1 + param2;
                            instructionPointer += 4;
                            break;
                        }
                    case 2: // multiply
                        {
                            var resultAddress = intCode[instructionPointer + 3];
                            var param1 = modeParam1 == 0 ? intCode[intCode[instructionPointer + 1]] : intCode[instructionPointer + 1];
                            var param2 = modeParam2 == 0 ? intCode[intCode[instructionPointer + 2]] : intCode[instructionPointer + 2];
                            intCode[resultAddress] = param1 * param2;
                            instructionPointer += 4;
                            break;
                        }
                    case 3: // input
                        {
                            var address = intCode[instructionPointer + 1];
                            intCode[address] = inputs[inputPointer];
                            inputPointer++;
                            instructionPointer += 2;
                            break;
                        }
                    case 4: // output
                        {
                            var param1 = modeParam1 == 0 ? intCode[intCode[instructionPointer + 1]] : intCode[instructionPointer + 1];
                            output.Add(param1);
                            instructionPointer += 2;
                            break;
                        }
                    case 5: // jump-if-true
                        {
                            var param1 = modeParam1 == 0 ? intCode[intCode[instructionPointer + 1]] : intCode[instructionPointer + 1];
                            var param2 = modeParam2 == 0 ? intCode[intCode[instructionPointer + 2]] : intCode[instructionPointer + 2];
                            instructionPointer = param1 != 0 ? param2 : instructionPointer + 3;
                            break;
                        }
                    case 6: // jump-if-false
                        {
                            var param1 = modeParam1 == 0 ? intCode[intCode[instructionPointer + 1]] : intCode[instructionPointer + 1];
                            var param2 = modeParam2 == 0 ? intCode[intCode[instructionPointer + 2]] : intCode[instructionPointer + 2];
                            instructionPointer = param1 == 0 ? param2 : instructionPointer + 3;
                            break;
                        }
                    case 7: // less than
                        {
                            var param1 = modeParam1 == 0 ? intCode[intCode[instructionPointer + 1]] : intCode[instructionPointer + 1];
                            var param2 = modeParam2 == 0 ? intCode[intCode[instructionPointer + 2]] : intCode[instructionPointer + 2];
                            var resultAddress = intCode[instructionPointer + 3];
                            intCode[resultAddress] = param1 < param2 ? 1 : 0;
                            instructionPointer += 4;
                            break;
                        }
                    case 8: // equals
                        {

                            var param1 = modeParam1 == 0 ? intCode[intCode[instructionPointer + 1]] : intCode[instructionPointer + 1];
                            var param2 = modeParam2 == 0 ? intCode[intCode[instructionPointer + 2]] : intCode[instructionPointer + 2];
                            var resultAddress = intCode[instructionPointer + 3];
                            intCode[resultAddress] = param1 == param2 ? 1 : 0;
                            instructionPointer += 4;
                            break;
                        }
                    case 99: // halt
                        return output;
                }
            }
        }

        private static int GetNumber(char character)
        {
            return character - '0';
        }
    }
}
