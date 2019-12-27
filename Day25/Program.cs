using IntCode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Day25
{
    public class Program
    {
        public static void Main()
        {
            // Solution items: hologram, jam, semiconductor, mutex
            // Additional items: polygon, prime number, monolith, weather machine
            // Unsafe items: infinite loop, photons, giant electromagnet, escape pod, molten lava

            var input = System.IO.File.ReadAllText("input.txt");

            var memoryState = input.Split(",")
                .Select(long.Parse)
                .ToList();

            var part1 = WhiteBoxSolution(memoryState);
            Console.WriteLine($"Part1 {part1}");
            Console.WriteLine("Press any key to play...");
            Console.ReadKey();

            var icc = new IntCodeComputer(memoryState);

            Console.Clear();

            while (!icc.IsHalted)
            {
                icc.RunIntCode(BreakMode.Input);

                PrintIntCodeMessage(icc);
                var inputMessage = Console.ReadLine() ?? string.Empty;

                inputMessage = inputMessage switch
                {
                    "n" => "north",
                    "s" => "south",
                    "w" => "west",
                    "e" => "east",
                    "i" => "inv",
                    _ => inputMessage
                };

                foreach (var character in inputMessage)
                {
                    icc.Inputs.Enqueue(character);
                }
                icc.Inputs.Enqueue(10);
            }

            PrintIntCodeMessage(icc);
        }

        private static void PrintIntCodeMessage(IntCodeComputer icc)
        {
            var outputMessage = new string(icc.Output.Select(c => (char)c).ToArray());
            icc.Output.Clear();
            Console.WriteLine(outputMessage);
        }

        private static int WhiteBoxSolution(IReadOnlyList<long> memoryState)
        {
            var testValue = memoryState[2486] * memoryState[1352];
            var binaryString = string.Join("", memoryState
                .Skip(1902)
                .Take(32)
                .Select(v => v >= testValue ? "1" : "0"));
            return Convert.ToInt32(binaryString, 2);
        }
    }
}
