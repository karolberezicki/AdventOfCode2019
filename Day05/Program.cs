using System;
using System.Collections.Generic;
using System.Linq;

namespace Day05
{
    public class Program
    {
        public static void Main()
        {
            var input = System.IO.File.ReadAllText("input.txt");
            var memoryState = input.Split(",")
                .Select(int.Parse)
                .ToList();

            var run1 = RunIntCode(memoryState, 1);
            var part1 = run1.Last();

            var run2 = RunIntCode(memoryState, 5);
            var part2 = run2.Last();

            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine($"Part2 {part2}");
        }

        private static List<int> RunIntCode(IEnumerable<int> memoryState, int inputValue)
        {
            var intCode = memoryState.ToList();
            var output = new List<int>();

            var instructionPointer = 0;

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
                            intCode[address] = inputValue;
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
